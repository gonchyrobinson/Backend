using Backend.DTOs.EmpresaDtos;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioEmpresas : IService<Empresa, EmpresaDto, EmpresaUpdateDto, EmpresaCreateDto>
    {
        Task<IEnumerable<EmpresaDto>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
    }
}
