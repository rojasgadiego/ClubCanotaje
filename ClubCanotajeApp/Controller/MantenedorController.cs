using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MantenedorController : ControllerBase
    {
        private readonly MantenedorService _service;
        public MantenedorController(MantenedorService service) => _service = service;

        [HttpGet("estados-canoa")]
        public async Task<IActionResult> GetEstadoCanoas() =>
            Ok(await _service.GetEstadoCanoasAsync());

        [HttpGet("tipos-canoa")]
        public async Task<IActionResult> GetTipoCanoas() =>
            Ok(await _service.GetTipoCanoasAsync());

        [HttpGet("motivos-cancelacion")]
        public async Task<IActionResult> GetMotivoCancelacion() =>
            Ok(await _service.GetMotivoCancelacionAsync());

        [HttpGet("roles-salida")]
        public async Task<IActionResult> GetRolEnSalidas() =>
            Ok(await _service.GetRolEnSalidasAsync());

        [HttpGet("tipos-membresia")]
        public async Task<IActionResult> GetTipoMembresias() =>
            Ok(await _service.GetTipoMembresiasAsync());
    }
}