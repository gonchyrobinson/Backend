using Backend.Models;
using Backend.DTOs.StudentDtos;

namespace Backend.Interfaces.Repositories
{
    public interface IRepositorioEstudiantes : IRepository<Estudiante>
    {
        Task<IEnumerable<Estudiante>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
        Task<IEnumerable<string>> GetSugerenciasApellidosAsync();
        Task<IEnumerable<object>> GetDocumentosUnicos();
    }
}
