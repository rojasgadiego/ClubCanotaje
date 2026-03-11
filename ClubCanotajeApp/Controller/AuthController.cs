using ClubCanotajeAPI.Models.Dtos.Common;
using ClubCanotajeAPI.Models.Dtos.Auth;
using ClubCanotajeAPI.Models.Dtos.Validacion;
using ClubCanotajeAPI.Services;
using ClubCanotajeAPI.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly IUsuarioService _usuarioService;

        public AuthController(IAuthService service, IUsuarioService usuarioService)
        {
            _service = service;
            _usuarioService = usuarioService;
        }

        /// <summary>Login → guarda JWT en cookie HttpOnly</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var (result, token) = await _service.LoginAsync(dto);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, //en dev
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });

            return Ok(ApiResponse<LoginResponse>.Ok(result, "Inicio de sesión exitoso."));
        }

        /// <summary>Logout → elimina cookie JWT</summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(ApiResponse<object>.Ok(null, "Sesión cerrada."));
        }

        /// <summary>Registro público</summary>
        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([FromBody] RegistroPublicoRequest dto)
        {
            var result = await _service.RegistroPublicoAsync(dto);
            return Ok(ApiResponse<RegistrarUsuarioResponse>.Ok(result, "Registro exitoso. Revisa tu email para verificar tu cuenta."));
        }

        /// <summary>Registro administrativo</summary>
        [HttpPost("registro/admin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistroAdmin([FromBody] RegistroAdminRequest dto)
        {
            var result = await _service.RegistroAdminAsync(dto);
            return Ok(ApiResponse<RegistrarUsuarioResponse>.Ok(result, "Usuario creado exitosamente."));
        }

        /// <summary>Verificar email con código de 6 dígitos</summary>
        [HttpPost("verificar-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarEmail([FromBody] VerificarEmailRequest dto)
        {
            await _service.VerificarEmailAsync(dto.Email, dto.Codigo);
            return Ok(ApiResponse<object>.Ok(null, "Email verificado correctamente. Ya puedes iniciar sesión."));
        }

        /// <summary>Reenviar código de verificación</summary>
        [HttpPost("reenviar-codigo")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarCodigo([FromBody] ReenviarCodigoRequest dto)
        {
            await _service.ReenviarCodigoAsync(dto.Email);
            return Ok(ApiResponse<object>.Ok(null, "Código de verificación reenviado. Revisa tu correo."));
        }

        /// <summary>Solicitar recuperación de contraseña</summary>
        [HttpPost("recuperar-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarPassword([FromBody] RecuperarPasswordRequest dto)
        {
            await _service.SolicitarResetPasswordAsync(dto.Email);
            return Ok(ApiResponse<object>.Ok(null, "Si el email existe, recibirás un código para restablecer tu contraseña."));
        }

        /// <summary>Restablecer contraseña con código</summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest dto)
        {
            await _service.ResetPasswordAsync(dto.Email, dto.Codigo, dto.NuevaPassword);
            return Ok(ApiResponse<object>.Ok(null, "Contraseña restablecida exitosamente. Ya puedes iniciar sesión."));
        }


        /// <summary>Retorna el usuario autenticado desde el JWT</summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (idClaim is null || !int.TryParse(idClaim, out var id))
                return Unauthorized(ApiResponse.Fail("Token inválido."));

            var result = await _usuarioService.GetInfoCompletaAsync(id);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}