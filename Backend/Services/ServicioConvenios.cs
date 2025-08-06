using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioConvenios : BaseService<Convenio, ConvenioDto, ConvenioCreateDto>
    {
        private readonly IRepositorioConvenios _repoConvenios;
        private readonly IRepositorioEmpresas _repoEmpresas;

        public ServicioConvenios(IRepositorioConvenios repoConvenios, IRepositorioEmpresas repoEmpresas, IMapper mapper)
            : base(repoConvenios, mapper)
        {
            _repoConvenios = repoConvenios;
            _repoEmpresas = repoEmpresas;
        }

        protected override int GetIdFromDto(ConvenioDto dto)
        {
            return dto.IdConvenio;
        }

        // Métodos específicos para convenios pueden agregarse aquí
        public async Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresaAsync()
        {
            return await _repoConvenios.ListarConveniosConEmpresaAsync();
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
                    throw new Backend.Exceptions.NotFoundException($"Empresa con ID {dto.IdEmpresa.Value} no encontrada");
                }
            }
            // Actualizar convenio
            return await base.UpdateAsync(dto);
        }
    }
}
