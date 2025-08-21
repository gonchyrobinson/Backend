using Backend.DTOs.EmpresaDtos;
using Backend.Models;

namespace Backend.Interfaces.Repositories
{
    public interface IRepositorioEmpresas : IRepository<Empresa>
    {
        public Task<IEnumerable<Empresa>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
    }
}
