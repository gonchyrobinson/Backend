using Backend.DTOs.AuditoriaDtos;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioAuditoria : IService<Auditoria, AuditoriaDto, AuditoriaUpdateDto, AuditoriaDto>
    {
        Task<IEnumerable<AuditoriaDto>> BuscarAsync(AuditoriaBuscarDto filtro);
        Task RegistrarAsync(AuditoriaDto dto);
    }
}
