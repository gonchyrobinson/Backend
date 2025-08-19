using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioEmpresas : BaseService<Empresa, EmpresaDto, EmpresaCreateDto>
    {
        private readonly IRepositorioEmpresas _repoEmpresas;
        private readonly IRepositorioConvenios _repoConvenios;
        private readonly EmpresaValidationService _validationService;

        public ServicioEmpresas(IRepositorioEmpresas repoEmpresas, IRepositorioConvenios repoConvenios, IMapper mapper)
            : base(repoEmpresas, mapper)
        {
            _repoEmpresas = repoEmpresas;
            _repoConvenios = repoConvenios;
            _validationService = new EmpresaValidationService(_repoConvenios);
        }

        protected override int GetIdFromDto(EmpresaDto dto)
        {
            return dto.IdEmpresa;
        }
        public async Task<IEnumerable<EmpresaDto>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro)
        {
            var empresas = await _repoEmpresas.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
        }

        public async Task<IEnumerable<string>> GetSugerenciasNombresAsync()
        {
            return await _repoEmpresas.GetSugerenciasNombresAsync();
        }
        public override async Task<bool> DeleteAsync(int id)
        {
            await _validationService.ValidateDeleteAsync(id);
            return await base.DeleteAsync(id);
        }
    }
}
