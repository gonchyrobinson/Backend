using Backend.DTOs.EmpresaDtos;
using Backend.Interfaces.Services;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class EmpresasController : BaseController<Empresa, EmpresaDto, EmpresaUpdateDto, EmpresaCreateDto>
    {
        private readonly IServicioEmpresas _empresasService;

        public EmpresasController(IServicioEmpresas service) : base(service)
        {
            _empresasService = service;
        }

        protected override int GetIdFromDto(EmpresaDto dto)
        {
            return dto.IdEmpresa;
        }

        [HttpPost("buscar/avanzado")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmpresaDto>>> BuscarAvanzado([FromBody] EmpresaBusquedaAvanzadaDto filtro)
        {
            var result = await _empresasService.BuscarAvanzadoAsync(filtro);
            return Ok(result);
        }

        [HttpGet("sugerencias-nombres")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetSugerenciasNombres()
        {
            var result = await _empresasService.GetSugerenciasNombresAsync();
            return Ok(result);
        }
    }
}
