using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioEstudiantes : IRepository<Estudiante>
    {
        Task<IEnumerable<Estudiante>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro);
    }
}
