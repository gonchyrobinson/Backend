using Backend.DTOs.ConvenioDtos;
using Backend.Interfaces.Services;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConveniosController : BaseController<Convenio, ConvenioDto, ConvenioUpdateDto, ConvenioCreateDto>
    {
        private readonly IServicioConvenios _conveniosService;

        public ConveniosController(IServicioConvenios service) : base(service)
        {
            _conveniosService = service;
        }

        protected override int GetIdFromDto(ConvenioDto dto)
        {
            return dto.IdConvenio;
        }

        // Métodos específicos para convenios pueden agregarse aquí

        [HttpPost("conEmpresa")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConvenioEmpresaDto>>> ListarConveniosConEmpresa([FromBody] ConvenioEmpresaFiltroDto filtro)
        {
            var result = await _conveniosService.ListarConveniosConEmpresaAsync(filtro);
            return Ok(result);
        }

        [HttpPost("asignar-empresa")]
        [Authorize]
        public async Task<IActionResult> AsignarEmpresa([FromBody] AsignarEmpresaDto dto)
        {
            var result = await _conveniosService.AsignarEmpresaAsync(dto);
            return Ok(result);
        }


        [HttpPost("caducar/{id}")]
        [Authorize]
        public async Task<ActionResult> CaducarConvenio(int id, [FromBody] CaducarConvenioDto dto)
        {
            DateOnly? fechaCaducidad = null;
            if (!string.IsNullOrEmpty(dto.FechaCaducidad))
            {
                if (DateOnly.TryParse(dto.FechaCaducidad, out var fecha))
                {
                    fechaCaducidad = fecha;
                }
                else
                {
                    return BadRequest("Formato de fecha inválido");
                }
            }

            var success = await _conveniosService.CaducarConvenioAsync(id, fechaCaducidad);
            if (success)
                return Ok();
            return BadRequest();
        }

    // Endpoints de sugerencias eliminados (dropdown y acuerdos marco)

        [HttpGet("empresas-convenio-vigente")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmpresaConvenioDropdownDto>>> GetEmpresasConUltimoConvenioVigente()
        {
            var result = await _conveniosService.GetEmpresasConUltimoConvenioVigenteAsync();
            return Ok(result);
        }

        // Endpoint: convenios por vencer
        [HttpGet("por-vencer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ConvenioEmpresaDto>>> GetConveniosPorVencer([FromQuery] int dias)
        {
            var convenios = await _conveniosService.GetConveniosPorVencerEnDiasAsync(dias);
            return Ok(convenios);
        }
    }
}
