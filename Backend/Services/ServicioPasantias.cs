using AutoMapper;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioPasantias : BaseService<Pasantia, PasantiaDto, PasantiaCreateDto>
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
            _validationService.ValidateCreate(dto);
            await _validationService.ValidateForeignKeysAsync(dto, _repoEstudiantes, _repoConvenios);
            var pasantiaDto = await base.CreateAsync(dto);
            var pagos = _validationService.GenerarPagosAutomaticos(dto, pasantiaDto.IdPasantia);
            foreach (var pago in pagos)
            {
                await _repoPasantias.AgregarPagoAsync(pago);
            }
            return pasantiaDto;
        }

        public override async Task<PasantiaDto> UpdateAsync(PasantiaDto dto)
        {
            var valoresValidos = new[] { "Pasantia", "PPS", "otro" };
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !valoresValidos.Contains(dto.TipoAcuerdo))
            {
                throw new ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", valoresValidos)}", "Pasantia");
            }
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await _repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await _repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }
            return await base.UpdateAsync(dto);
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

        public async Task<IEnumerable<PasantiaDto>> BuscarAvanzadoAsync(PasantiaBusquedaAvanzadaDto filtro)
        {
            var entities = await _repoPasantias.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }
    }
}
