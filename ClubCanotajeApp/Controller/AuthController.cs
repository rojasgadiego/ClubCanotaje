using ClubCanotajeAPI.Models.Dtos.Auth;
using ClubCanotajeAPI.Models.Dtos.Validacion;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service) => _service = service;

        /// <summary>Login → devuelve JWT</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var result = await _service.LoginAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Registro público — cualquier persona puede registrarse.
        /// Crea automáticamente un Remador + Usuario con rol Remador.
        /// </summary>
        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([FromBody] RegistroPublicoRequest dto)
        {
            var result = await _service.RegistroPublicoAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Registro administrativo — solo un Administrador puede usarlo.
        /// Crea un usuario con cualquier rol sin vincular remador.
        /// </summary>
        [HttpPost("registro/admin")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RegistroAdmin([FromBody] RegistroAdminRequest dto)
        {
            var result = await _service.RegistroAdminAsync(dto);
            return Ok(result);
        }

        /// <summary>Verificar email con código de 6 dígitos</summary>
        [HttpPost("verificar-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarEmail([FromBody] VerificarEmailRequest dto)
        {
            await _service.VerificarEmailAsync(dto.Email, dto.Codigo);
            return Ok();
        }

        /// <summary>Reenviar código de verificación</summary>
        [HttpPost("reenviar-codigo")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarCodigo([FromBody] ReenviarCodigoRequest dto)
        {
            await _service.ReenviarCodigoAsync(dto.Email);
            return Ok();
        }

        /// <summary>Solicitar recuperación de contraseña</summary>
        [HttpPost("recuperar-password")]
        [AllowAnonymous]
        public async Task<IActionResult> RecuperarPassword([FromBody] RecuperarPasswordRequest dto)
        {
            await _service.SolicitarResetPasswordAsync(dto.Email);
            return Ok();
        }

        /// <summary>Restablecer contraseña con código</summary>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest dto)
        {
            await _service.ResetPasswordAsync(dto.Email, dto.Codigo, dto.NuevaPassword);
            return Ok();
        }
    }
}