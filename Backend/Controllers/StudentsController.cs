using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    public class StudentsController : BaseController<Student, StudentDto>
    {
        public StudentsController(StudentService service) : base(service)
        {
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.Id;
        }
    }
}