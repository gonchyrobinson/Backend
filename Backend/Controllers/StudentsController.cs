using Backend.DTOs.StudentDtos;
using Backend.Interfaces.Services;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : BaseController<Estudiante, StudentDto, StudentUpdateDto, StudentCreateDto>
    {
        private readonly IServicioEstudiantes _studentService;

        public StudentsController(IServicioEstudiantes service) : base(service)
        {
            _studentService = service;
        }

        [HttpPost("buscar-avanzado")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StudentDto>>> BuscarAvanzado([FromBody] StudentBusquedaAvanzadaDto filtro)
        {
            var result = await _studentService.BuscarAvanzadoAsync(filtro);
            return Ok(result);
        }

        [HttpGet("sugerencias-nombres")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetSugerenciasNombres()
        {
            var result = await _studentService.GetSugerenciasNombresAsync();
            return Ok(result);
        }

        [HttpGet("sugerencias-apellidos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetSugerenciasApellidos()
        {
            var result = await _studentService.GetSugerenciasApellidosAsync();
            return Ok(result);
        }

        [HttpGet("documentos-dropdown")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<object>>> GetDocumentosDropdown()
        {
            var result = await _studentService.GetDocumentosUnicos();
            return Ok(result);
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}