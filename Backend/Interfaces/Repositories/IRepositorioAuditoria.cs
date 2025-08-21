using Backend.DTOs.AuditoriaDtos;
using Backend.Models;

namespace Backend.Interfaces.Repositories
{
    public interface IRepositorioAuditoria : IRepository<Auditoria>
    {
        Task<IEnumerable<Auditoria>> BuscarAsync(AuditoriaBuscarDto filtro);
    }
}
