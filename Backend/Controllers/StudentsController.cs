using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : BaseController<Estudiante, StudentDto, StudentCreateDto>
    {
        private readonly ServicioEstudiantes _studentService;

        public StudentsController(ServicioEstudiantes service) : base(service)
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

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}