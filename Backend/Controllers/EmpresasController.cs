using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
public class EmpresasController : BaseController<Empresa, EmpresaDto, EmpresaCreateDto>
    {
        private readonly ServicioEmpresas _empresasService;

    public EmpresasController(ServicioEmpresas service) : base(service)
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
    }
}
