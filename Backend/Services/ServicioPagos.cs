using AutoMapper;
using Backend.DTOs.PagosDtos;
using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Models;
namespace Backend.Services
{
    public class ServicioPagos : BaseService<Pago, PagosDto, PagosUpdateDto, CreatePagosDto>, IServicioPagos
    {
        private readonly IRepositorioPagos _repoPagos;
        private readonly IRepository<Pasantia> _repoPasantias;
        private readonly PagoValidationService _validationService;

        public ServicioPagos(IRepositorioPagos repoPagos, IRepository<Pasantia> repoPasantias, IMapper mapper)
            : base(repoPagos, mapper)
        {
            _repoPagos = repoPagos;
            _repoPasantias = repoPasantias;
            _validationService = new PagoValidationService(_repoPasantias);
        }

        public override async Task<PagosDto> CreateAsync(CreatePagosDto dto)
        {
            await _validationService.ValidateCreateAsync(dto);
            return await base.CreateAsync(dto);
        }

        public override async Task<PagosDto> UpdateAsync(PagosUpdateDto dto)
        {
            // Convertir UpdateDto a PagosDto para validación
            var pagosDto = _mapper.Map<PagosDto>(dto);
            await _validationService.ValidateUpdateAsync(pagosDto);
            return await base.UpdateAsync(dto);
        }

        protected override int GetIdFromUpdateDto(PagosUpdateDto dto)
        {
            return dto.IdPago;
        }


        // Pagos por vencer en X días desde hoy
        public async Task<IEnumerable<PagosDto>> GetPagosPorVencerEnDiasAsync(int dias)
        {
            var pagos = await _repoPagos.GetPagosPorVencerEnDiasAsync(dias);
            return _mapper.Map<IEnumerable<PagosDto>>(pagos);
        }

        public async Task<IEnumerable<PagosDto>> GetByPasantiaIdAsync(int idPasantia)
        {
            var pagos = await _repoPagos.GetByPasantiaIdAsync(idPasantia);
            return _mapper.Map<IEnumerable<PagosDto>>(pagos);
        }

        public async Task<PagosDto> MarcarComoPagadoAsync(MarcarPagoDto dto)
        {
            var pago = await _repoPagos.MarcarComoPagadoAsync(dto.IdPago, dto.FechaPago);
            return _mapper.Map<PagosDto>(pago);
        }

        public async Task<IEnumerable<PagosDto>> BuscarAvanzadoAsync(PagosBusquedaAvanzadaDto filtro)
        {
            var pagos = await _repoPagos.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<PagosDto>>(pagos);
        }

        public async Task<IEnumerable<object>> GetSugerenciasEmpresasAsync()
        {
            return await _repoPagos.GetSugerenciasEmpresasAsync();
        }

        public async Task<IEnumerable<object>> GetSugerenciasEstudiantesAsync()
        {
            return await _repoPagos.GetSugerenciasEstudiantesAsync();
        }

        public async Task<IEnumerable<string>> GetSugerenciasTramitesSudocuAsync()
        {
            return await _repoPagos.GetSugerenciasTramitesSudocuAsync();
        }
    }
}
