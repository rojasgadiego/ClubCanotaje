using ClubCanotajeAPI.Models.Dtos.Auth;
using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Entities;
using ClubCanotajeAPI.Repositories.Usuario;
using ClubCanotajeAPI.Repositories.Verificacion;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClubCanotajeAPI.Services
{
    public class AuthService
    {
        private readonly UsuarioRepository _usuarioRepo;
        private readonly VerificacionRepository _verificacionRepo;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(
            UsuarioRepository usuarioRepo,
            VerificacionRepository verificacionRepo,
            EmailService emailService,
            IConfiguration config)
        {
            _usuarioRepo = usuarioRepo;
            _verificacionRepo = verificacionRepo;
            _emailService = emailService;
            _config = config;
        }

        // ── Login ─────────────────────────────────────────────────

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest dto)
        {
            var usuario = await _usuarioRepo.GetByUsernameAsync(dto.Username);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                return ApiResponse<LoginResponse>.Fail("Usuario o contraseña incorrectos.");

            if (!usuario.Activo)
                return ApiResponse<LoginResponse>.Fail("Usuario inactivo. Contacta al administrador.");

            if (!usuario.EmailVerificado)
                return ApiResponse<LoginResponse>.Fail(
                    "Debes verificar tu email antes de iniciar sesión. Revisa tu correo.");

            await _usuarioRepo.RegistrarAccesoAsync(usuario.Id);

            var nombre = usuario.Remador?.NombreCompleto
                      ?? usuario.Instructor?.NombreCompleto
                      ?? usuario.Username;

            var token = GenerarToken(usuario);
            var expira = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("JwtSettings:ExpirationMinutes", 60));

            return ApiResponse<LoginResponse>.Ok(
                new LoginResponse(usuario.Id, token, expira, usuario.Username, nombre, usuario.Rol.Nombre));
        }

        // Registro público

        public async Task<ApiResponse<RegistrarUsuarioResponse>> RegistroPublicoAsync(RegistroPublicoRequest dto)
        {
            if (await _usuarioRepo.ExisteUsernameAsync(dto.Username))
                return ApiResponse<RegistrarUsuarioResponse>.Fail(
                    $"El username '{dto.Username}' ya está en uso.");

            if (await _usuarioRepo.ExisteRutAsync(dto.Rut))
                return ApiResponse<RegistrarUsuarioResponse>.Fail(
                    "Ya existe un remador registrado con ese RUT.");

            if (await _usuarioRepo.ExisteEmailRemadorAsync(dto.Email))
                return ApiResponse<RegistrarUsuarioResponse>.Fail(
                    "Ya existe un remador registrado con ese email.");

            var rol = await _usuarioRepo.GetRolByNombreAsync("Remador");
            if (rol is null)
                return ApiResponse<RegistrarUsuarioResponse>.Fail(
                    "Error de configuración: rol 'Remador' no encontrado.");

            var idCategoria = await _usuarioRepo.GetCategoriaDefaultAsync();
            var idEstado = await _usuarioRepo.GetEstadoRemadorActivoAsync();

            var remador = new Remador
            {
                Rut = dto.Rut.Trim(),
                Nombres = dto.Nombres.Trim(),
                ApellidoPaterno = dto.ApellidoPaterno.Trim(),
                ApellidoMaterno = dto.ApellidoMaterno?.Trim(),
                FechaNacimiento = dto.FechaNacimiento,
                Genero = dto.Genero,
                Email = dto.Email.Trim().ToLower(),
                Telefono = dto.Telefono,
                IdCategoria = idCategoria,
                IdEstado = idEstado,
                FechaIngreso = DateOnly.FromDateTime(DateTime.Today),
                FechaCreacion = DateTime.Now
            };

            var usuario = new UsuarioSistema
            {
                Username = dto.Username.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IdRol = rol.Id,
                Activo = false,
                EmailVerificado = false,
                FechaCreacion = DateTime.Now
            };

            var creado = await _usuarioRepo.CrearConRemadorAsync(remador, usuario);

            // Generar y enviar código
            var codigo = await _verificacionRepo.CrearCodigoAsync(
                remador.Email, TipoVerificacion.Registro);

            await _emailService.EnviarCodigoVerificacionAsync(
                remador.Email, codigo.Codigo, TipoVerificacion.Registro);

            return ApiResponse<RegistrarUsuarioResponse>.Ok(
                new RegistrarUsuarioResponse(creado.Id, creado.Username, rol.Nombre),
                "Registro exitoso. Revisa tu email para verificar tu cuenta.");
        }

        // Registro admin

        public async Task<ApiResponse<RegistrarUsuarioResponse>> RegistroAdminAsync(RegistroAdminRequest dto)
        {
            if (await _usuarioRepo.ExisteUsernameAsync(dto.Username))
                return ApiResponse<RegistrarUsuarioResponse>.Fail(
                    $"El username '{dto.Username}' ya está en uso.");

            var rol = await _usuarioRepo.GetRolByIdAsync(dto.IdRol);
            if (rol is null)
                return ApiResponse<RegistrarUsuarioResponse>.Fail("El rol especificado no existe.");

            var usuario = new UsuarioSistema
            {
                Username = dto.Username.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IdRol = dto.IdRol,
                Activo = true,
                EmailVerificado = true,
                FechaCreacion = DateTime.Now
            };

            var creado = await _usuarioRepo.CrearAsync(usuario);

            return ApiResponse<RegistrarUsuarioResponse>.Ok(
                new RegistrarUsuarioResponse(creado.Id, creado.Username, rol.Nombre),
                $"Usuario '{creado.Username}' creado con rol '{rol.Nombre}'.");
        }

        // Verificar email

        public async Task<ApiResponse> VerificarEmailAsync(string email, string codigo)
        {
            var verif = await _verificacionRepo.ValidarCodigoAsync(
                email, codigo, TipoVerificacion.Registro);

            if (verif is null)
                return ApiResponse.Fail("Código inválido o expirado. Solicita uno nuevo.");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email);
            if (usuario is null)
                return ApiResponse.Fail("Usuario no encontrado.");

            if (usuario.EmailVerificado)
                return ApiResponse.Fail("Este email ya fue verificado.");

            await _usuarioRepo.ActivarUsuarioAsync(usuario);
            await _verificacionRepo.MarcarComoUsadoAsync(verif);

            return ApiResponse.Ok("Email verificado correctamente. Ya puedes iniciar sesión.");
        }

        // Reenviar código

        public async Task<ApiResponse> ReenviarCodigoAsync(string email)
        {
            var remador = await _usuarioRepo.GetRemadorByEmailAsync(email);
            if (remador is null)
                return ApiResponse.Fail("Email no registrado.");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email);
            if (usuario is null)
                return ApiResponse.Fail("Usuario no encontrado.");

            if (usuario.EmailVerificado)
                return ApiResponse.Fail("Este email ya está verificado.");

            var codigo = await _verificacionRepo.CrearCodigoAsync(
                email, TipoVerificacion.Registro);

            await _emailService.EnviarCodigoVerificacionAsync(
                email, codigo.Codigo, TipoVerificacion.Registro);

            return ApiResponse.Ok("Código reenviado. Revisa tu email.");
        }

        // Recuperar contraseña

        public async Task<ApiResponse> SolicitarResetPasswordAsync(string email)
        {
            var remador = await _usuarioRepo.GetRemadorByEmailAsync(email);

            if (remador is null)
                return ApiResponse.Ok(
                    "Si el email está registrado, recibirás un código de recuperación.");

            var codigo = await _verificacionRepo.CrearCodigoAsync(
                email, TipoVerificacion.ResetPassword, 30);

            await _emailService.EnviarCodigoVerificacionAsync(
                email, codigo.Codigo, TipoVerificacion.ResetPassword);

            return ApiResponse.Ok(
                "Si el email está registrado, recibirás un código de recuperación.");
        }

        public async Task<ApiResponse> ResetPasswordAsync(
            string email, string codigo, string nuevaPassword)
        {
            var verif = await _verificacionRepo.ValidarCodigoAsync(
                email, codigo, TipoVerificacion.ResetPassword);

            if (verif is null)
                return ApiResponse.Fail("Código inválido o expirado.");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email);
            if (usuario is null)
                return ApiResponse.Fail("Usuario no encontrado.");

            var nuevoHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
            await _usuarioRepo.CambiarPasswordAsync(usuario, nuevoHash);

            await _verificacionRepo.MarcarComoUsadoAsync(verif);

            return ApiResponse.Ok("Contraseña actualizada correctamente.");
        }

        // Listar roles

        public async Task<ApiResponse<List<RolDto>>> GetRolesAsync()
        {
            var roles = await _usuarioRepo.GetRolesAsync();
            return ApiResponse<List<RolDto>>.Ok(
                roles.Select(r => new RolDto(r.Id, r.Nombre)).ToList());
        }

        // Generar JWT

        private string GenerarToken(UsuarioSistema usuario)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("JwtSettings:ExpirationMinutes", 60));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name,           usuario.Username),
                new Claim(ClaimTypes.Role,           usuario.Rol.Nombre),
                new Claim("id_remador",              usuario.Remador?.ToString() ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public record RolDto(int Id, string Nombre);
}