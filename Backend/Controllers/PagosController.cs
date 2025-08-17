using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : BaseController<Pago, PagosDto, CreatePagosDto>
    {
        private readonly ServicioPagos _pagosService;

        public PagosController(ServicioPagos pagosService) : base(pagosService)
        {
            _pagosService = pagosService;
        }

        protected override int GetIdFromDto(PagosDto dto)
        {
            return dto.IdPago;
        }


        // Endpoint: pagos por vencer
        [HttpGet("por-vencer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PagosDto>>> GetPagosPorVencer([FromQuery] DateOnly fecha)
        {
            var pagos = await _pagosService.GetPagosPorVencerAsync(fecha);
            return Ok(pagos);
        }

        [HttpGet("by-pasantia/{idPasantia}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PagosDto>>> GetByPasantiaId(int idPasantia)
        {
            var pagosDto = await _pagosService.GetByPasantiaIdAsync(idPasantia);
            return Ok(pagosDto);
        }

        [HttpPost("marcar-pagado")]
        [Authorize]
        public async Task<ActionResult<PagosDto>> MarcarComoPagado([FromBody] MarcarPagoDto dto)
        {
            var pagoDto = await _pagosService.MarcarComoPagadoAsync(dto);
            return Ok(pagoDto);
        }
    }
}
