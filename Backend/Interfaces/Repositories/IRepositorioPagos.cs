using Backend.DTOs.PagosDtos;
using Backend.Models;
namespace Backend.Interfaces.Repositories
{
    public interface IRepositorioPagos : IRepository<Pago>
    {
        Task<IEnumerable<Pago>> GetByPasantiaIdAsync(int idPasantia);
        Task<Pago> MarcarComoPagadoAsync(int idPago, DateOnly? fechaPago = null);
        Task<IEnumerable<Pago>> GetPagosPorVencerEnDiasAsync(int dias);
        Task<IEnumerable<Pago>> BuscarAvanzadoAsync(PagosBusquedaAvanzadaDto filtro);
        Task<IEnumerable<object>> GetSugerenciasEmpresasAsync();
        Task<IEnumerable<object>> GetSugerenciasEstudiantesAsync();
    }
}