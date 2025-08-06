using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IRepositorioConvenios : IRepository<Convenio>
    {
        // Listar convenios junto a empresa
        Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresaAsync();

        // Caducar convenio
        Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad);
    }
}
