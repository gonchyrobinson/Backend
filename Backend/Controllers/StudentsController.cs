using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    public class StudentsController : BaseController<Estudiante, StudentDto>
    {
        public StudentsController(ServicioEstudiantes service) : base(service)
        {
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}