using Backend.Models;
namespace Backend.Interfaces
{
    public interface IRepositorioPagos : IRepository<Pago>
    {
        Task<Pago> GetByPasantiaIdAsync(int idPasantia);
    }
}