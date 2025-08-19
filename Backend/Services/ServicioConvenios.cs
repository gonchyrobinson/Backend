using AutoMapper;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioConvenios : BaseService<Convenio, ConvenioDto, ConvenioCreateDto>
    {
        private readonly IRepositorioConvenios _repoConvenios;
        private readonly IRepositorioEmpresas _repoEmpresas;
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly ConvenioValidationService _validationService;

        public ServicioConvenios(IRepositorioConvenios repoConvenios, IRepositorioEmpresas repoEmpresas, IRepositorioPasantias repoPasantias, IMapper mapper)
            : base(repoConvenios, mapper)
        {
            _repoConvenios = repoConvenios;
            _repoEmpresas = repoEmpresas;
            _repoPasantias = repoPasantias;
            _validationService = new ConvenioValidationService(_repoConvenios, _repoPasantias);
        }

        protected override int GetIdFromDto(ConvenioDto dto)
        {
            return dto.IdConvenio;
        }

        // Métodos específicos para convenios pueden agregarse aquí

        public override async Task<bool> DeleteAsync(int id)
        {
            await _validationService.ValidateDeleteAsync(id);
            return await base.DeleteAsync(id);
        }
        public async Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresaAsync(ConvenioEmpresaFiltroDto filtro)
        {
            return await _repoConvenios.ListarConveniosConEmpresa(filtro);
        }


        public async Task<bool> AsignarEmpresaAsync(AsignarEmpresaDto dto)
        {
            // Validar existencia de la empresa
            var empresa = await _repoEmpresas.GetByIdAsync(dto.EmpresaId);
            if (empresa == null)
                throw new NotFoundException($"Empresa con ID {dto.EmpresaId} no encontrada");
            return await _repoConvenios.AsignarEmpresaAsync(dto);
        }

        public async Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad = null)
        {
            return await _repoConvenios.CaducarConvenioAsync(convenioId, fechaCaducidad);
        }

        public override async Task<ConvenioDto> UpdateAsync(ConvenioDto dto)
        {
            // Validar existencia de empresa si se especifica IdEmpresa
            if (dto.IdEmpresa.HasValue)
            {
                var empresa = await _repoEmpresas.GetByIdAsync(dto.IdEmpresa.Value);
                if (empresa == null)
                {
                    throw new NotFoundException($"Empresa con ID {dto.IdEmpresa.Value} no encontrada");
                }
            }
            // Actualizar convenio
            return await base.UpdateAsync(dto);
        }
    }
}
