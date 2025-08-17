using Backend.Models;
namespace Backend.Interfaces
{
    public interface IRepositorioPagos : IRepository<Pago>
    {
        Task<IEnumerable<Pago>> GetByPasantiaIdAsync(int idPasantia);
        Task<Pago> MarcarComoPagadoAsync(int idPago, DateOnly? fechaPago = null);
        Task<IEnumerable<Pago>> GetPagosPorVencerEnDiasAsync(int dias);
    }
}