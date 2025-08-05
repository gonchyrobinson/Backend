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

        public async Task<IEnumerable<StudentDto>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro)
        {
            var estudiantes = await _repoEstudiantes.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<StudentDto>>(estudiantes);
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}