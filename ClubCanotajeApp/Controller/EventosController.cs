using ClubCanotajeAPI.Models.Dtos.Evento;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventosController : ControllerBase
{
    private readonly EventoService _service;
    public EventosController(EventoService service) => _service = service;

    // ── Consultar eventos ─────────────────────────────────────

    /// <summary>Todos los eventos</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Eventos próximos (no finalizados ni cancelados)</summary>
    [HttpGet("proximos")]
    public async Task<IActionResult> GetProximos() =>
        Ok(await _service.GetProximosAsync());

    /// <summary>Detalle completo de un evento con inscripciones</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Tabla de resultados de un evento</summary>
    [HttpGet("{id:int}/resultados")]
    public async Task<IActionResult> GetResultados(int id)
    {
        var result = await _service.GetResultadosAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // ── Gestión de eventos (Admin) ────────────────────────────

    /// <summary>Crear nuevo evento</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> Crear([FromBody] CrearEventoDto dto)
    {
        var result = await _service.CrearAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>Actualizar datos del evento</summary>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEventoDto dto)
    {
        var result = await _service.ActualizarAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Cambiar estado manualmente (usar endpoints específicos preferiblemente)</summary>
    [HttpPatch("{id:int}/estado")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoEventoDto dto)
    {
        var result = await _service.CambiarEstadoAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Flujo del evento ──────────────────────────────────────

    /// <summary>Abrir inscripciones (Planificado → Inscripciones abiertas)</summary>
    [HttpPost("{id:int}/abrir-inscripciones")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> AbrirInscripciones(int id)
    {
        var result = await _service.AbrirInscripcionesAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Cerrar inscripciones</summary>
    [HttpPost("{id:int}/cerrar-inscripciones")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> CerrarInscripciones(int id)
    {
        var result = await _service.CerrarInscripcionesAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Iniciar evento (valida que haya participantes)</summary>
    [HttpPost("{id:int}/iniciar")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> Iniciar(int id)
    {
        var result = await _service.IniciarEventoAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Finalizar evento (En curso → Finalizado)</summary>
    [HttpPost("{id:int}/finalizar")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> Finalizar(int id)
    {
        var result = await _service.FinalizarEventoAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Inscripciones ─────────────────────────────────────────

    /// <summary>Inscribir equipo en el evento</summary>
    [HttpPost("{id:int}/inscribir-equipo")]
    [Authorize(Roles = "Administrador,Directiva,Entrenador")]
    public async Task<IActionResult> InscribirEquipo(int id, [FromBody] InscribirEquipoDto dto)
    {
        var result = await _service.InscribirEquipoAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Inscribir remador individual en el evento</summary>
    [HttpPost("{id:int}/inscribir-remador")]
    [Authorize(Roles = "Administrador,Directiva,Entrenador,Remador")]
    public async Task<IActionResult> InscribirRemador(int id, [FromBody] InscribirRemadorDto dto)
    {
        var result = await _service.InscribirRemadorAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Confirmar inscripción de un participante</summary>
    [HttpPost("inscripciones/{idInscripcion:int}/confirmar")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> ConfirmarInscripcion(int idInscripcion)
    {
        var result = await _service.ConfirmarInscripcionAsync(idInscripcion);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Asignar números de largada (antes de iniciar)</summary>
    [HttpPost("{id:int}/asignar-largadas")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> AsignarLargadas(
        int id, [FromBody] AsignarNumerosLargadaDto dto)
    {
        var result = await _service.AsignarNumerosLargadaAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // ── Resultados ────────────────────────────────────────────

    /// <summary>Registrar o actualizar resultado de un participante</summary>
    [HttpPost("resultados")]
    [Authorize(Roles = "Administrador,Directiva")]
    public async Task<IActionResult> RegistrarResultado([FromBody] RegistrarResultadoDto dto)
    {
        var result = await _service.RegistrarResultadoAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}