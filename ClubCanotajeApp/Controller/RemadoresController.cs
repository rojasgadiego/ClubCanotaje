using ClubCanotajeAPI.Models.Dtos.Remador;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubCanotajeAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RemadoresController : ControllerBase
    {
        private readonly RemadorService _service;
        public RemadoresController(RemadorService service) => _service = service;

        /// <summary>Lista todos los remadores con estado de membresía</summary>
        [HttpGet]
        [Authorize(Roles = "Administrador,Directiva,Entrenador")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        /// <summary>Detalle de un remador</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Un remador solo puede ver su propio perfil salvo Admin/Entrenador
            var idToken = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var esAdmin = User.IsInRole("Administrador") || User.IsInRole("Directiva") || User.IsInRole("Entrenador");
            if (!esAdmin && idToken != id) return Forbid();

            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        /// <summary>Crear remador</summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear([FromBody] CrearRemadorDto dto)
        {
            var result = await _service.CrearAsync(dto);
            if (!result.Success) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        /// <summary>Actualizar datos propios o del remador (Admin)</summary>
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarRemadorDto dto)
        {
            var idToken = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var esAdmin = User.IsInRole("Administrador") || User.IsInRole("Directiva");
            if (!esAdmin && idToken != id) return Forbid();

            var result = await _service.ActualizarAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
