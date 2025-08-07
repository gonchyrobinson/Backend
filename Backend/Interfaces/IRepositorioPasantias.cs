using Backend.Models;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Backend.Interfaces
{
    public interface IRepositorioPasantias : IRepository<Pasantia>
    {
        // Métodos específicos para pasantías pueden agregarse aquí

        Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync();
        Task<IEnumerable<Pasantia>> GetByConvenioIdAsync(int convenioId);
        Task<IEnumerable<Pasantia>> GetByEstudianteIdAsync(int estudianteId);
    }
}
