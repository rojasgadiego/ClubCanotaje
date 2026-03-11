using ClubCanotajeAPI.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClubCanotajeAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service) => _service = service;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInfoCompleta(int id)
    {
        var result = await _service.GetInfoCompletaAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// GET api/usuarios/me
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMiPerfil()
    {
        // Lee el id desde el JWT, no desde la URL
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idClaim is null || !int.TryParse(idClaim, out int id))
            return Unauthorized(new { message = "Token inválido." });

        var result = await _service.GetInfoCompletaAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }
}