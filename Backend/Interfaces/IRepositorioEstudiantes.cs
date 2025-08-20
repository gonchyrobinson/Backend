using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioEstudiantes : IRepository<Estudiante>
    {
        Task<IEnumerable<Estudiante>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
        Task<IEnumerable<string>> GetSugerenciasApellidosAsync();
        Task<IEnumerable<object>> GetDocumentosUnicos();
    }
}
