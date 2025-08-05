using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioEstudiantes : BaseService<Estudiante, StudentDto>
    {
        private readonly IRepositorioEstudiantes _repoEstudiantes;

        public ServicioEstudiantes(IRepositorioEstudiantes repository, IMapper mapper) : base(repository, mapper)
        {
            _repoEstudiantes = repository;
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}