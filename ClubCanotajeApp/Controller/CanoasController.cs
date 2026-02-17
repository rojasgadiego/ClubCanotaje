using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CanoasController : ControllerBase
    {
        private readonly CanoaService _service;
        public CanoasController(CanoaService service) => _service = service;

        /// <summary>Todas las canoas con estado</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        /// <summary>Solo canoas disponibles para reservar ahora</summary>
        [HttpGet("disponibles")]
        public async Task<IActionResult> GetDisponibles() => Ok(await _service.GetDisponiblesAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Retorna canoas disponibles para un horario específico.
        /// Query params:
        /// - fechaHora: formato ISO 8601 (ej: 2025-02-20T14:00:00)
        /// - duracionMin: duración estimada en minutos (default: 120)
        /// </summary>
        [HttpGet("disponibles/horario")]
        public async Task<IActionResult> GetDisponiblesPorHorario(
            [FromQuery] DateTime fechaHora,
            [FromQuery] int duracionMin = 120)
        {
            var result = await _service.GetDisponiblesPorHorarioAsync(fechaHora, duracionMin);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
