using ClubCanotajeAPI.Models.Dtos.Implemento;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImplementosController : ControllerBase
{
    private readonly ImplementoService _service;
    public ImplementosController(ImplementoService service) => _service = service;

    /// <summary>Todos los implementos con su estado actual</summary>
    [HttpGet]
    [Authorize(Roles = "Administrador,Directiva,Entrenador")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Solo implementos disponibles para asignar</summary>
    [HttpGet("disponibles")]
    public async Task<IActionResult> GetDisponibles() =>
        Ok(await _service.GetDisponiblesAsync());

    /// <summary>Disponibles filtrados por tipo (Remo, Salvavidas, etc.)</summary>
    [HttpGet("disponibles/tipo/{idTipo:int}")]
    public async Task<IActionResult> GetDisponiblesPorTipo(int idTipo) =>
        Ok(await _service.GetDisponiblesPorTipoAsync(idTipo));

    /// <summary>Detalle de un implemento</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Registrar nuevo implemento</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearImplementoDto dto)
    {
        var result = await _service.CrearAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>Actualizar datos del implemento</summary>
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Administrador,Entrenador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarImplementoDto dto)
    {
        var result = await _service.ActualizarAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Cambiar estado (Disponible, Mantenimiento, Dado de baja)</summary>
    [HttpPatch("{id:int}/estado")]
    [Authorize(Roles = "Administrador,Entrenador")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoImplementoDto dto)
    {
        var result = await _service.CambiarEstadoAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Asignar implemento a una salida activa</summary>
    [HttpPost("salida/{idSalida:int}/asignar")]
    [Authorize(Roles = "Administrador,Entrenador")]
    public async Task<IActionResult> Asignar(int idSalida, [FromBody] AsignarImplementoDto dto)
    {
        var result = await _service.AsignarASalidaAsync(idSalida, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Registrar devolución de un implemento</summary>
    [HttpPost("salida/{idSalida:int}/devolver/{idImplemento:int}")]
    [Authorize(Roles = "Administrador,Entrenador")]
    public async Task<IActionResult> Devolver(int idSalida, int idImplemento)
    {
        var result = await _service.DevolverAsync(idSalida, idImplemento);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}