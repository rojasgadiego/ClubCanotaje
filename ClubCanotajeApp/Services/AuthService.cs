using ClubCanotajeAPI.Exceptions;
using ClubCanotajeAPI.Models.Dtos.Auth;
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
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UsuarioRepository usuarioRepo,
            VerificacionRepository verificacionRepo,
            EmailService emailService,
            IConfiguration config,
            ILogger<AuthService> logger)
        {
            _usuarioRepo = usuarioRepo;
            _verificacionRepo = verificacionRepo;
            _emailService = emailService;
            _config = config;
            _logger = logger;
        }

        // ── Login ─────────────────────────────────────────────────

        public async Task<LoginResponse> LoginAsync(LoginRequest dto)
        {
            var usuario = await _usuarioRepo.GetByUsernameAsync(dto.Username);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                throw new DataSourceException("Usuario o contraseña incorrectos.", "001");

            if (!usuario.Activo)
                throw new DataSourceException("Usuario inactivo. Contacta al administrador.", "002");

            if (!usuario.EmailVerificado)
                throw new DataSourceException(
                    "Debes verificar tu email antes de iniciar sesión. Revisa tu correo.", "003");

            await _usuarioRepo.RegistrarAccesoAsync(usuario.Id);

            var nombre = usuario.Remador?.NombreCompleto
                      ?? usuario.Instructor?.NombreCompleto
                      ?? usuario.Username;

            var token = GenerarToken(usuario);
            var expira = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("JwtSettings:ExpirationMinutes", 60));

            return new LoginResponse(usuario.Id, token, expira, usuario.Username, nombre, usuario.Rol.Nombre);
        }

        // ── Registro público ──────────────────────────────────────

        public async Task<RegistrarUsuarioResponse> RegistroPublicoAsync(RegistroPublicoRequest dto)
        {
            if (await _usuarioRepo.ExisteUsernameAsync(dto.Username))
                throw new DataSourceException($"El username '{dto.Username}' ya está en uso.", "004");

            if (await _usuarioRepo.ExisteRutAsync(dto.Rut))
                throw new DataSourceException("Ya existe un remador registrado con ese RUT.", "005");

            if (await _usuarioRepo.ExisteEmailRemadorAsync(dto.Email))
                throw new DataSourceException("Ya existe un remador registrado con ese email.", "006");

            var rol = await _usuarioRepo.GetRolByNombreAsync("Remador")
                ?? throw new ModelException("Error de configuración: rol 'Remador' no encontrado.", "001");

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

            // Email fuera de la transacción — fallo no revierte el registro
            await EnviarCodigoAsync(remador.Email, TipoVerificacion.Registro, creado.Id);

            return new RegistrarUsuarioResponse(creado.Id, creado.Username, rol.Nombre);
        }

        // ── Registro admin ────────────────────────────────────────

        public async Task<RegistrarUsuarioResponse> RegistroAdminAsync(RegistroAdminRequest dto)
        {
            if (await _usuarioRepo.ExisteUsernameAsync(dto.Username))
                throw new DataSourceException($"El username '{dto.Username}' ya está en uso.", "004");

            var rol = await _usuarioRepo.GetRolByIdAsync(dto.IdRol)
                ?? throw new DataSourceException("El rol especificado no existe.", "007");

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

            return new RegistrarUsuarioResponse(creado.Id, creado.Username, rol.Nombre);
        }

        // ── Verificar email ───────────────────────────────────────

        public async Task VerificarEmailAsync(string email, string codigo)
        {
            var verif = await _verificacionRepo.ValidarCodigoAsync(email, codigo, TipoVerificacion.Registro)
                ?? throw new DataSourceException("Código inválido o expirado. Solicita uno nuevo.", "008");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email)
                ?? throw new ModelException("Usuario no encontrado.", "002");

            if (usuario.EmailVerificado)
                throw new DataSourceException("Este email ya fue verificado.", "009");

            await _usuarioRepo.ActivarUsuarioAsync(usuario);
            await _verificacionRepo.MarcarComoUsadoAsync(verif);
        }

        // ── Reenviar código ───────────────────────────────────────

        public async Task ReenviarCodigoAsync(string email)
        {
            var remador = await _usuarioRepo.GetRemadorByEmailAsync(email)
                ?? throw new DataSourceException("Email no registrado.", "010");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email)
                ?? throw new ModelException("Usuario no encontrado.", "002");

            if (usuario.EmailVerificado)
                throw new DataSourceException("Este email ya está verificado.", "009");

            await EnviarCodigoAsync(email, TipoVerificacion.Registro, usuario.Id);
        }

        // ── Recuperar contraseña ──────────────────────────────────

        public async Task SolicitarResetPasswordAsync(string email)
        {
            // Siempre retorna sin error para no revelar si el email existe
            var remador = await _usuarioRepo.GetRemadorByEmailAsync(email);
            if (remador is null) return;

            await EnviarCodigoAsync(email, TipoVerificacion.ResetPassword, minutos: 30);
        }

        public async Task ResetPasswordAsync(string email, string codigo, string nuevaPassword)
        {
            var verif = await _verificacionRepo.ValidarCodigoAsync(email, codigo, TipoVerificacion.ResetPassword)
                ?? throw new DataSourceException("Código inválido o expirado.", "008");

            var usuario = await _usuarioRepo.GetByEmailRemadorAsync(email)
                ?? throw new ModelException("Usuario no encontrado.", "002");

            var nuevoHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
            await _usuarioRepo.CambiarPasswordAsync(usuario, nuevoHash);
            await _verificacionRepo.MarcarComoUsadoAsync(verif);
        }

        // ── Roles ─────────────────────────────────────────────────

        public async Task<List<RolDto>> GetRolesAsync()
        {
            var roles = await _usuarioRepo.GetRolesAsync();
            return roles.Select(r => new RolDto(r.Id, r.Nombre)).ToList();
        }

        // ── Helpers privados ──────────────────────────────────────

        /// <summary>
        /// Genera y envía un código de verificación por email.
        /// Si el envío falla, loguea el error y lanza ClientException.
        /// El usuario ya quedó creado — esto NO revierte la BD.
        /// </summary>
        private async Task EnviarCodigoAsync(
            string email,
            string tipo,
            int? idUsuario = null,
            int minutos = 15)
        {
            try
            {
                var codigo = await _verificacionRepo.CrearCodigoAsync(email, tipo, minutos);
                await _emailService.EnviarCodigoVerificacionAsync(email, codigo.Codigo, tipo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Fallo al enviar email de verificación. Tipo: {Tipo} | Email: {Email} | UsuarioId: {Id}",
                    tipo, email, idUsuario);

                throw new ClientException(
                    "No se pudo enviar el email de verificación. Intenta reenviar el código.", "001");
            }
        }

        private string GenerarToken(UsuarioSistema usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddMinutes(
                _config.GetValue<int>("JwtSettings:ExpirationMinutes", 60));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name,           usuario.Username),
                new Claim(ClaimTypes.Role,           usuario.Rol.Nombre),
                new Claim("id_remador",              usuario.Remador?.Id.ToString() ?? "")
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