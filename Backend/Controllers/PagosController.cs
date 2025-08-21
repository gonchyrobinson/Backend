using Backend.DTOs.PagosDtos;
using Backend.Interfaces.Services;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : BaseController<Pago, PagosDto, PagosUpdateDto, CreatePagosDto>
    {
        private readonly IServicioPagos _pagosService;

        public PagosController(IServicioPagos pagosService) : base(pagosService)
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
        public async Task<ActionResult<IEnumerable<PagosDto>>> GetPagosPorVencer([FromQuery] int dias)
        {
            var pagos = await _pagosService.GetPagosPorVencerEnDiasAsync(dias);
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
