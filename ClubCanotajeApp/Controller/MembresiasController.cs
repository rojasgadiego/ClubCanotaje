using ClubCanotajeAPI.Models.Dtos.Membresia;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MembresiasController : ControllerBase
    {
        private readonly MembresiaService _service;
        public MembresiasController(MembresiaService service) => _service = service;

        /// <summary>Membresía vigente de un remador</summary>
        [HttpGet("remador/{idRemador:int}")]
        public async Task<IActionResult> GetVigente(int idRemador)
        {
            var result = await _service.GetVigenteAsync(idRemador);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>Crear membresía para un remador</summary>
        [HttpPost]
        [Authorize(Roles = "Administrador,Directiva")]
        public async Task<IActionResult> Crear([FromBody] CrearMembresiaDto dto)
        {
            var result = await _service.CrearAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>Cuotas pendientes o vencidas — vista de cobranza</summary>
        [HttpGet("cuotas/pendientes")]
        [Authorize(Roles = "Administrador,Directiva")]
        public async Task<IActionResult> GetCuotasPendientes() =>
            Ok(await _service.GetCuotasPendientesAsync());

        /// <summary>Registrar pago de una cuota</summary>
        [HttpPost("cuotas/pago")]
        [Authorize(Roles = "Administrador,Directiva")]
        public async Task<IActionResult> RegistrarPago([FromBody] RegistrarPagoDto dto)
        {
            var idRegistrador = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.RegistrarPagoAsync(dto, idRegistrador);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
