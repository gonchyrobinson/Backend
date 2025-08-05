using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;

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
        public async Task<ActionResult<IEnumerable<StudentDto>>> BuscarAvanzado([FromBody] StudentBusquedaAvanzadaDto filtro)
        {
            var result = await _studentService.BuscarAvanzadoAsync(filtro);
            return Ok(result);
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}