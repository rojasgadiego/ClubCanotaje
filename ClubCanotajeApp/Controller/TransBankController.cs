using ClubCanotajeAPI.Models.Dtos.Transbank.Request;
using ClubCanotajeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubCanotajeAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransBankController : ControllerBase
    {
        private readonly TransBankService _service;
        public TransBankController(TransBankService service) => _service = service;

        [HttpPost("crearTransaccion")]
        [AllowAnonymous]
        public IActionResult createTransaction([FromBody] CreateTransaction request)
        {
            try
            {
                var result = _service.CrearTransaccion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear la transacción" + ex.Message);
            }
        }

        [HttpPost("confirmarTransaccion")]
        [AllowAnonymous]
        public IActionResult confirmTransaction([FromBody] ConfirmTransaction request)
        {
            try
            {
                var result = _service.confirmrTransaccion(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear la confirmar transacción" + ex.Message);
            }
        }


    }
}
