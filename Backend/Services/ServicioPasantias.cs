using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioPasantias : BaseService<Pasantia, PasantiaDto, PasantiaCreateDto>
    {
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioConvenios _repoConvenios;

        public ServicioPasantias(IRepositorioPasantias repoPasantias, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios, IMapper mapper)
            : base(repoPasantias, mapper)
        {
            _repoPasantias = repoPasantias;
            _repoEstudiantes = repoEstudiantes;
            _repoConvenios = repoConvenios;
        }

        protected override int GetIdFromDto(PasantiaDto dto)
        {
            return dto.IdPasantia;
        }

        // Métodos específicos para pasantías pueden agregarse aquí
        public async Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync()
        {
            return await _repoPasantias.GetAllDetalleAsync();
        }


        public async Task<IEnumerable<PasantiaDto>> GetByConvenioIdAsync(int convenioId)
        {
            var entities = await _repoPasantias.GetByConvenioIdAsync(convenioId);
            if (entities == null || !entities.Any())
                throw new Backend.Exceptions.NotFoundException($"No se encontraron pasantías para el convenio con ID {convenioId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }

        public async Task<IEnumerable<PasantiaDto>> GetByEstudianteIdAsync(int estudianteId)
        {
            var entities = await _repoPasantias.GetByEstudianteIdAsync(estudianteId);
            if (entities == null || !entities.Any())
                throw new Backend.Exceptions.NotFoundException($"No se encontraron pasantías para el estudiante con ID {estudianteId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }
        public override async Task<PasantiaDto> CreateAsync(PasantiaCreateDto dto)
        {
            // Validar ENUM tipo_acuerdo
            var valoresValidos = new[] { "Pasantia", "PPS", "otro" };
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !valoresValidos.Contains(dto.TipoAcuerdo))
            {
                throw new Backend.Exceptions.ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", valoresValidos)}", "Pasantia");
            }
            // Validar claves foráneas
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await _repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new Backend.Exceptions.NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await _repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new Backend.Exceptions.NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }
            return await base.CreateAsync(dto);
        }

        public override async Task<PasantiaDto> UpdateAsync(PasantiaDto dto)
        {
            var valoresValidos = new[] { "Pasantia", "PPS", "otro" };
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !valoresValidos.Contains(dto.TipoAcuerdo))
            {
                throw new Backend.Exceptions.ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", valoresValidos)}", "Pasantia");
            }
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await _repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new Backend.Exceptions.NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await _repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new Backend.Exceptions.NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }
            return await base.UpdateAsync(dto);
        }
    }
}
