using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioEmpresas : IRepository<Empresa>
    {
        public Task<IEnumerable<Empresa>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
    }
}
