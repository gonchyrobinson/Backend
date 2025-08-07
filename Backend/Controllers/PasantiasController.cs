using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasantiasController : BaseController<Pasantia, PasantiaDto, PasantiaCreateDto>
    {
        private readonly ServicioPasantias _pasantiasService;

        public PasantiasController(ServicioPasantias service) : base(service)
        {
            _pasantiasService = service;
        }

        protected override int GetIdFromDto(PasantiaDto dto)
        {
            return dto.IdPasantia;
        }

        // Métodos específicos para pasantías pueden agregarse aquí

        [HttpGet("detalle")]
        public async Task<ActionResult<IEnumerable<PasantiaDetalleDto>>> GetAllDetalle()
        {
            var result = await _pasantiasService.GetAllDetalleAsync();
            return Ok(result);
        }

        [HttpGet("convenio/{convenioId}")]
        public async Task<ActionResult<IEnumerable<PasantiaDto>>> GetByConvenioId(int convenioId)
        {
            var result = await _pasantiasService.GetByConvenioIdAsync(convenioId);
            return Ok(result);
        }

        [HttpGet("estudiante/{estudianteId}")]
        public async Task<ActionResult<IEnumerable<PasantiaDto>>> GetByEstudianteId(int estudianteId)
        {
            var result = await _pasantiasService.GetByEstudianteIdAsync(estudianteId);
            return Ok(result);
        }
    }
}
