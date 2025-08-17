using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioPasantias : IRepository<Pasantia>
    {
        // Métodos específicos para pasantías pueden agregarse aquí

        Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync();
        Task<IEnumerable<Pasantia>> GetByConvenioIdAsync(int convenioId);
        Task<IEnumerable<Pasantia>> GetByEstudianteIdAsync(int estudianteId);

        // Nuevo: agregar pago asociado a una pasantía
        Task AgregarPagoAsync(Pago pago);
    }
}
