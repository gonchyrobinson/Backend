using Backend.DTOs.ConvenioDtos;
using Backend.Models;

namespace Backend.Interfaces.Repositories
{
    public interface IRepositorioConvenios : IRepository<Convenio>
    {
        // Listar convenios junto a empresa
        Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresa(ConvenioEmpresaFiltroDto filtro);

        // Asignar empresa a convenio
        Task<bool> AsignarEmpresaAsync(AsignarEmpresaDto dto);

        // Caducar convenio
        Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad);
        
        // Método para obtener sugerencias de convenios para dropdown
        Task<IEnumerable<object>> GetSugerenciasDropdownAsync();

        // Método para obtener empresas con último convenio vigente (para dropdown de pasantías)
        Task<IEnumerable<EmpresaConvenioDropdownDto>> GetEmpresasConUltimoConvenioVigenteAsync();

        // Método para obtener convenios por vencer en días
        Task<IEnumerable<ConvenioEmpresaDto>> GetConveniosPorVencerEnDiasAsync(int dias);
    }
}
