using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class StudentService : BaseService<Student, StudentDto>
    {
        public StudentService(StudentRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}