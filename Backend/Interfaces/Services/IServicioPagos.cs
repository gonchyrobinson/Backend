using Backend.DTOs.PagosDtos;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioPagos : IService<Pago, PagosDto, PagosUpdateDto, CreatePagosDto>
    {
        Task<IEnumerable<PagosDto>> GetPagosPorVencerEnDiasAsync(int dias);
        Task<IEnumerable<PagosDto>> GetByPasantiaIdAsync(int idPasantia);
        Task<PagosDto> MarcarComoPagadoAsync(MarcarPagoDto dto);
    }
}
