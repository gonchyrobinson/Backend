using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
namespace Backend.Services
{
    public class ServicioPagos : BaseService<Pago, PagosDto, CreatePagosDto>
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

        public override async Task<PagosDto> UpdateAsync(PagosDto dto)
        {
            await _validationService.ValidateUpdateAsync(dto);
            return await base.UpdateAsync(dto);
        }

        protected override int GetIdFromDto(PagosDto dto)
        {
            return dto.IdPago;
        }


        // Pagos por vencer hasta una fecha
        public async Task<IEnumerable<PagosDto>> GetPagosPorVencerAsync(DateOnly fecha)
        {
            var pagos = await _repoPagos.GetPagosPorVencerAsync(fecha);
            return _mapper.Map<IEnumerable<PagosDto>>(pagos);
        }

        public async Task<PagosDto> GetByPasantiaIdAsync(int idPasantia)
        {
            var pago = await _repoPagos.GetByPasantiaIdAsync(idPasantia);
            return _mapper.Map<PagosDto>(pago);
        }

        public async Task<PagosDto> MarcarComoPagadoAsync(MarcarPagoDto dto)
        {
            var pago = await _repoPagos.MarcarComoPagadoAsync(dto.IdPago, dto.FechaPago);
            return _mapper.Map<PagosDto>(pago);
        }
    }
}
