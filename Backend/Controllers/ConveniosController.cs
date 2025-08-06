using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConveniosController : BaseController<Convenio, ConvenioDto, ConvenioCreateDto>
    {
        private readonly ServicioConvenios _conveniosService;

        public ConveniosController(ServicioConvenios service) : base(service)
        {
            _conveniosService = service;
        }

        protected override int GetIdFromDto(ConvenioDto dto)
        {
            return dto.IdConvenio;
        }

        // Métodos específicos para convenios pueden agregarse aquí

        [HttpGet("con-empresa")]
        public async Task<ActionResult<IEnumerable<ConvenioEmpresaDto>>> ListarConveniosConEmpresa()
        {
            var result = await _conveniosService.ListarConveniosConEmpresaAsync();
            return Ok(result);
        }

        [HttpPost("caducar/{id}")]
        public async Task<ActionResult> CaducarConvenio(int id, [FromBody] DateOnly? fechaCaducidad = null)
        {
            var success = await _conveniosService.CaducarConvenioAsync(id, fechaCaducidad);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
