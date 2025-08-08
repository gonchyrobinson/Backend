using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Métodos específicos para pagos pueden agregarse aquí

        [HttpGet("by-pasantia/{idPasantia}")]
        [Authorize]
        public async Task<ActionResult<PagosDto?>> GetByPasantiaId(int idPasantia)
        {
            var pagoDto = await _pagosService.GetByPasantiaIdAsync(idPasantia);
            return Ok(pagoDto);
        }
    }
}
