using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ServicioEstudiantes : BaseService<Estudiante, StudentDto>
    {
        public ServicioEstudiantes(RepositorioEstudiantes repository, IMapper mapper) : base(repository, mapper)
        {
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}