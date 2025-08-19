using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioAuditoria : IRepository<Auditoria>
    {
        Task<IEnumerable<Auditoria>> BuscarAsync(AuditoriaBuscarDto filtro);
    }
}
