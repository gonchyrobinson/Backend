using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioEstudiantes : BaseService<Estudiante, StudentDto, StudentCreateDto>
    {
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly EstudianteValidationService _validationService;

        public ServicioEstudiantes(
            IRepositorioEstudiantes repository,
            IMapper mapper,
            EstudianteValidationService validationService) : base(repository, mapper)
        {
            _repoEstudiantes = repository;
            _validationService = validationService;
        }

        public async Task<IEnumerable<StudentDto>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro)
        {
            var estudiantes = await _repoEstudiantes.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<StudentDto>>(estudiantes);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await _validationService.ValidateDeleteAsync(id);
            return await base.DeleteAsync(id);
        }

        protected override int GetIdFromDto(StudentDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}