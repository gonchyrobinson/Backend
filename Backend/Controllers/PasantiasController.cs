using Backend.DTOs.PasantiaDtos;
using Backend.Interfaces.Services;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasantiasController : BaseController<Pasantia, PasantiaDto, PasantiaUpdateDto, PasantiaCreateDto>
    {
        private readonly IServicioPasantias _pasantiasService;

        public PasantiasController(IServicioPasantias service) : base(service)
        {
            _pasantiasService = service;
        }

        protected override int GetIdFromDto(PasantiaDto dto)
        {
            return dto.IdPasantia;
        }

        // Métodos específicos para pasantías pueden agregarse aquí

        [HttpGet("detalle")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaDetalleDto>>> GetAllDetalle()
        {
            var result = await _pasantiasService.GetAllDetalleAsync();
            return Ok(result);
        }

        [HttpGet("convenio/{convenioId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaDto>>> GetByConvenioId(int convenioId)
        {
            var result = await _pasantiasService.GetByConvenioIdAsync(convenioId);
            return Ok(result);
        }

        [HttpGet("estudiante/{estudianteId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaDto>>> GetByEstudianteId(int estudianteId)
        {
            var result = await _pasantiasService.GetByEstudianteIdAsync(estudianteId);
            return Ok(result);
        }

        [HttpGet("sugerencias-tramites")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetSugerenciasTramites()
        {
            var result = await _pasantiasService.GetSugerenciasTramitesAsync();
            return Ok(result);
        }

        [HttpGet("sugerencias-dropdown")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetSugerenciasDropdown()
        {
            var result = await _pasantiasService.GetSugerenciasDropdownAsync();
            return Ok(result);
        }

        [HttpGet("sugerencias-numeros-tramite")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetSugerenciasNumerosTramite()
        {
            var result = await _pasantiasService.GetSugerenciasNumerosTramiteAsync();
            return Ok(result);
        }

        [HttpPost("buscar-avanzado")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaShowTableDto>>> BuscarAvanzado([FromBody] PasantiaBusquedaAvanzadaDto filtro)
        {
            var result = await _pasantiasService.BuscarAvanzadoAsync(filtro);
            return Ok(result);
        }

        // Endpoint: datos para tabla de pasantías
        [HttpGet("show-table")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaShowTableDto>>> GetAllPasantiasShowTable()
        {
            var result = await _pasantiasService.GetAllPasantiasShowTableAsync();
            return Ok(result);
        }

        // Endpoint: pasantías por vencer
        [HttpGet("por-vencer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PasantiaDto>>> GetPasantiasPorVencer([FromQuery] int dias)
        {
            var pasantias = await _pasantiasService.GetPasantiasPorVencerEnDiasAsync(dias);
            return Ok(pasantias);
        }
    }
}
