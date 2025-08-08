using Backend.Models;
using Backend.DTOs;

namespace Backend.Interfaces
{
    public interface IRepositorioAuditoria : IRepository<Auditoria>
    {
        Task<IEnumerable<Auditoria>> BuscarAsync(AuditoriaBuscarDto filtro);
    }
}
