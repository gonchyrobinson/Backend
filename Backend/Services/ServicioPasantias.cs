using AutoMapper;
using Backend.DTOs.PasantiaDtos;
using Backend.Exceptions;
using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioPasantias : BaseService<Pasantia, PasantiaDto, PasantiaUpdateDto, PasantiaCreateDto>, IServicioPasantias
    {
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioConvenios _repoConvenios;
        private readonly IRepositorioPagos _repoPagos;
        private readonly PasantiaValidationService _validationService;

        public ServicioPasantias(IRepositorioPasantias repoPasantias, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios, IRepositorioPagos repoPagos, IMapper mapper)
            : base(repoPasantias, mapper)
        {
            _repoPasantias = repoPasantias;
            _repoEstudiantes = repoEstudiantes;
            _repoConvenios = repoConvenios;
            _repoPagos = repoPagos;
            _validationService = new PasantiaValidationService();
        }

        protected override int GetIdFromUpdateDto(PasantiaUpdateDto dto)
        {
            return dto.IdPasantia;
        }

        // Sobrescribir métodos base para incluir navegaciones
        public override async Task<IEnumerable<PasantiaDto>> GetAllAsync()
        {
            var entities = await _repoPasantias.GetAllWithStudentNavigationAsync();
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }

        public override async Task<PasantiaDto> GetByIdAsync(int id)
        {
            var entity = await _repoPasantias.GetByIdWithStudentNavigationAsync(id);
            if (entity == null)
                throw new NotFoundException($"Pasantia con ID {id} no encontrada");
            return _mapper.Map<PasantiaDto>(entity);
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
                throw new NotFoundException($"No se encontraron pasantias para el convenio con ID {convenioId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }

        public async Task<IEnumerable<PasantiaDto>> GetByEstudianteIdAsync(int estudianteId)
        {
            var entities = await _repoPasantias.GetByEstudianteIdAsync(estudianteId);
            if (entities == null || !entities.Any())
                throw new NotFoundException($"No se encontraron pasantias para el estudiante con ID {estudianteId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }

        public override async Task<PasantiaDto> CreateAsync(PasantiaCreateDto dto)
        {
            // Buscar el estudiante por DNI si se proporciona
            int? estudianteId = null;
            if (!string.IsNullOrEmpty(dto.DniEstudiante))
            {
                var estudiante = await _repoEstudiantes.GetByDocumentoAsync(dto.DniEstudiante);
                if (estudiante == null)
                {
                    throw new NotFoundException($"No se encontró un estudiante con DNI {dto.DniEstudiante}");
                }
                estudianteId = estudiante.IdEstudiante;
            }

            _validationService.ValidateCreate(dto);
            await _validationService.ValidateLegalRequirementsAsync(dto, _repoPasantias, _repoEstudiantes, _repoConvenios, estudianteId);
            await _validationService.ValidateForeignKeysAsync(estudianteId, dto.IdConvenio, _repoEstudiantes, _repoConvenios);
            
            // Crear una entidad Pasantia manualmente para incluir el IdEstudiante encontrado
            var pasantia = _mapper.Map<Pasantia>(dto);
            pasantia.IdEstudiante = estudianteId;
            
            var result = await _repoPasantias.AddAsync(pasantia);
            var pasantiaDto = _mapper.Map<PasantiaDto>(result);
            
            var pagos = _validationService.GenerarPagosAutomaticos(dto, pasantiaDto.IdPasantia);
            foreach (var pago in pagos)
            {
                await _repoPasantias.AgregarPagoAsync(pago);
            }
            return pasantiaDto;
        }

        public override async Task<PasantiaDto> UpdateAsync(PasantiaUpdateDto dto)
        {
            // Buscar el estudiante por DNI si se proporciona
            int? estudianteId = null;
            if (!string.IsNullOrEmpty(dto.DniEstudiante))
            {
                var estudiante = await _repoEstudiantes.GetByDocumentoAsync(dto.DniEstudiante);
                if (estudiante == null)
                {
                    throw new NotFoundException($"No se encontró un estudiante con DNI {dto.DniEstudiante}");
                }
                estudianteId = estudiante.IdEstudiante;
            }

            // Usar la validación específica para updates con DNI
            _validationService.ValidateUpdateWithDni(dto, estudianteId);
            await _validationService.ValidateForeignKeysAsync(estudianteId, dto.IdConvenio, _repoEstudiantes, _repoConvenios);
            
            // Crear una entidad Pasantia manualmente para incluir el IdEstudiante encontrado
            var pasantia = _mapper.Map<Pasantia>(dto);
            pasantia.IdEstudiante = estudianteId;
            pasantia.IdPasantia = dto.IdPasantia; // Asignar el ID directamente
            
            var result = await _repository.UpdateAsync(pasantia);
            return _mapper.Map<PasantiaDto>(result);
        }

        // Pasantías por vencer en X días desde hoy
        public async Task<IEnumerable<PasantiaDto>> GetPasantiasPorVencerEnDiasAsync(int dias)
        {
            var pasantias = await _repoPasantias.GetPasantiasPorVencerEnDiasAsync(dias);
            return _mapper.Map<IEnumerable<PasantiaDto>>(pasantias);
        }

        // Obtener datos para mostrar en tabla
        public async Task<IEnumerable<PasantiaShowTableDto>> GetAllPasantiasShowTableAsync()
        {
            return await _repoPasantias.GetAllPasantiasShowTableAsync();
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            await _validationService.ValidateDeleteAsync(id, _repoPagos, _repoPasantias);
            return await base.DeleteAsync(id);
        }

        public async Task<IEnumerable<string>> GetSugerenciasTramitesAsync()
        {
            return await _repoPasantias.GetSugerenciasTramitesAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasNumerosTramiteAsync()
        {
            return await _repoPasantias.GetSugerenciasNumerosTramiteAsync();
        }

        public async Task<IEnumerable<object>> GetSugerenciasDropdownAsync()
        {
            return await _repoPasantias.GetSugerenciasDropdownAsync();
        }

        public async Task<IEnumerable<PasantiaShowTableDto>> BuscarAvanzadoAsync(PasantiaBusquedaAvanzadaDto filtro)
        {
            return await _repoPasantias.BuscarAvanzadoAsync(filtro);
        }
    }
}
