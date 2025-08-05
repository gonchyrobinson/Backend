using Backend.DTOs;
using Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IRepositorioEmpresas : IRepository<Empresa>
    {
       public Task<IEnumerable<Empresa>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro);
    }
}
