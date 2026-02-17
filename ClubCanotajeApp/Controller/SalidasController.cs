using ClubCanotajeAPI.Models.Dtos.Salida;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalidasController : ControllerBase
    {
        private readonly SalidaService _service;
        public SalidasController(SalidaService service) => _service = service;

        /// <summary>Agenda: próximas reservas pendientes</summary>
        [HttpGet("proximas")]
        public async Task<IActionResult> GetProximas() => Ok(await _service.GetProximasAsync());

        /// <summary>Historial de salidas finalizadas</summary>
        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial(
            [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta) =>
            Ok(await _service.GetHistorialAsync(desde, hasta));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>Crear reserva anticipada</summary>
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDto dto)
        {
            var idResponsable = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.CrearReservaAsync(dto, idResponsable);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        /// <summary>Agregar participante a una reserva</summary>
        [HttpPost("{id:int}/participantes")]
        public async Task<IActionResult> AgregarParticipante(
            int id, [FromBody] AgregarParticipanteDto dto)
        {
            var result = await _service.AgregarParticipanteAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>Iniciar salida → canoa al agua</summary>
        [HttpPost("{id:int}/iniciar")]
        [Authorize(Roles = "Administrador,Entrenador")]
        public async Task<IActionResult> Iniciar(int id, [FromBody] IniciarSalidaDto dto)
        {
            var result = await _service.IniciarAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>Finalizar salida → libera canoa e implementos</summary>
        [HttpPost("{id:int}/finalizar")]
        [Authorize(Roles = "Administrador,Entrenador")]
        public async Task<IActionResult> Finalizar(int id, [FromBody] FinalizarSalidaDto dto)
        {
            var result = await _service.FinalizarAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>Cancelar una reserva</summary>
        [HttpPost("{id:int}/cancelar")]
        public async Task<IActionResult> Cancelar(int id, [FromBody] CancelarReservaDto dto)
        {
            var canceladoPor = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _service.CancelarAsync(id, dto, canceladoPor);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
