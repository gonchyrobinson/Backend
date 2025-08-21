using AutoMapper;
using Backend.DTOs.StudentDtos;
using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioEstudiantes : BaseService<Estudiante, StudentDto, StudentUpdateDto, StudentCreateDto>, IServicioEstudiantes
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

        public async Task<IEnumerable<string>> GetSugerenciasNombresAsync()
        {
            return await _repoEstudiantes.GetSugerenciasNombresAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasApellidosAsync()
        {
            return await _repoEstudiantes.GetSugerenciasApellidosAsync();
        }

        public async Task<IEnumerable<object>> GetDocumentosUnicos()
        {
            return await _repoEstudiantes.GetDocumentosUnicos();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await _validationService.ValidateDeleteAsync(id);
            return await base.DeleteAsync(id);
        }

        protected override int GetIdFromUpdateDto(StudentUpdateDto dto)
        {
            return dto.IdEstudiante;
        }
    }
}