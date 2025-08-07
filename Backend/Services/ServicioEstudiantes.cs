using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioEstudiantes : BaseService<Estudiante, StudentDto, StudentCreateDto>
    {
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioPasantias _repoPasantias;

        public ServicioEstudiantes(IRepositorioEstudiantes repository, IRepositorioPasantias repoPasantias, IMapper mapper) : base(repository, mapper)
        {
            _repoEstudiantes = repository;
            _repoPasantias = repoPasantias;
        }

        public async Task<IEnumerable<StudentDto>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro)
        {
            var estudiantes = await _repoEstudiantes.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<StudentDto>>(estudiantes);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            // Verificar si existe alguna pasantía asociada al estudiante
            var estudiante = await _repoEstudiantes.GetByIdAsync(id);
            if (estudiante == null)
                throw new Backend.Exceptions.NotFoundException($"Estudiante con ID {id} no encontrado");

            var pasantias = await _repoPasantias.GetByEstudianteIdAsync(id);
            if (pasantias.Any())
            {
                throw new Backend.Exceptions.ValidationException($"No se puede eliminar el estudiante porque tiene pasantías asociadas.", "Estudiante");
            }
            return await base.DeleteAsync(id);
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}