using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Exceptions;
namespace Backend.Services
{
    public class ServicioPagos : BaseService<Pago, PagosDto, CreatePagosDto>
    {
        private readonly IRepositorioPagos _repoPagos;

        // Inyección del repositorio de pasantías para validación
        private readonly IRepository<Pasantia> _repoPasantias;

        public ServicioPagos(IRepositorioPagos repoPagos, IRepository<Pasantia> repoPasantias, IMapper mapper)
            : base(repoPagos, mapper)
        {
            _repoPagos = repoPagos;
            _repoPasantias = repoPasantias;
        }
        public override async Task<PagosDto> CreateAsync(CreatePagosDto dto)
        {
            // Validar que IdPasantia no sea null
            if (dto.IdPasantia != null)
            {
                var pasantia = await _repoPasantias.GetByIdAsync(dto.IdPasantia.Value);
                if (pasantia == null)
                    throw new Exceptions.ValidationException($"No existe una pasantía con ID {dto.IdPasantia}");
            }
            return await base.CreateAsync(dto);
        }

        public override async Task<PagosDto> UpdateAsync(PagosDto dto)
        {
            // Validar que IdPasantia no sea null
            if (dto.IdPasantia != null)
            {
                var pasantia = await _repoPasantias.GetByIdAsync(dto.IdPasantia.Value);
                if (pasantia == null)
                    throw new Exceptions.ValidationException($"No existe una pasantía con ID {dto.IdPasantia}");
            }
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
