using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly ServicioEmpresas _servicioEmpresas;
        private readonly IMapper _mapper;

        public EmpresasController(ServicioEmpresas servicioEmpresas, IMapper mapper)
        {
            _servicioEmpresas = servicioEmpresas;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpresaDto>>> GetEmpresas()
        {
            var empresas = await _servicioEmpresas.GetAllAsync();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmpresaDto>> GetEmpresa(int id)
        {
            var empresa = await _servicioEmpresas.GetByIdAsync(id);
            if (empresa == null)
                return NotFound();
            return Ok(empresa);
        }

        [HttpPost]
        public async Task<ActionResult<EmpresaDto>> CreateEmpresa([FromBody] EmpresaDto empresaDto)
        {
            var created = await _servicioEmpresas.AddAsync(empresaDto);
            return CreatedAtAction(nameof(GetEmpresa), new { id = created.IdEmpresa }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmpresaDto>> UpdateEmpresa(int id, [FromBody] EmpresaDto empresaDto)
        {
            if (id != empresaDto.IdEmpresa)
                return BadRequest();
            var updated = await _servicioEmpresas.UpdateAsync(empresaDto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var deleted = await _servicioEmpresas.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }

        [HttpGet("buscar/avanzado")]
        public async Task<ActionResult<IEnumerable<EmpresaDto>>> BuscarAvanzado(
            [FromQuery] string? nombre,
            [FromQuery] string? vigencia,
            [FromQuery] string? tipoContrato,
            [FromQuery] string? fechaInicioDesde,
            [FromQuery] string? fechaInicioHasta,
            [FromQuery] string? fechaFinDesde,
            [FromQuery] string? fechaFinHasta)
        {
            DateOnly? fechaInicioDesdeVal = null;
            DateOnly? fechaInicioHastaVal = null;
            DateOnly? fechaFinDesdeVal = null;
            DateOnly? fechaFinHastaVal = null;
            if (DateOnly.TryParse(fechaInicioDesde, out var fiDesde)) fechaInicioDesdeVal = fiDesde;
            if (DateOnly.TryParse(fechaInicioHasta, out var fiHasta)) fechaInicioHastaVal = fiHasta;
            if (DateOnly.TryParse(fechaFinDesde, out var ffDesde)) fechaFinDesdeVal = ffDesde;
            if (DateOnly.TryParse(fechaFinHasta, out var ffHasta)) fechaFinHastaVal = ffHasta;
            var result = await _servicioEmpresas.BuscarAvanzadoAsync(nombre, vigencia, tipoContrato, fechaInicioDesdeVal, fechaInicioHastaVal, fechaFinDesdeVal, fechaFinHastaVal);
            return Ok(result);
        }
    }
}
