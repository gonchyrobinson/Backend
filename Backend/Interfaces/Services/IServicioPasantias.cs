using Backend.DTOs.PasantiaDtos;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioPasantias : IService<Pasantia, PasantiaDto, PasantiaUpdateDto, PasantiaCreateDto>
    {
        Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync();
        Task<IEnumerable<PasantiaDto>> GetByConvenioIdAsync(int convenioId);
        Task<IEnumerable<PasantiaDto>> GetByEstudianteIdAsync(int estudianteId);
        Task<IEnumerable<string>> GetSugerenciasTramitesAsync();
        Task<IEnumerable<string>> GetSugerenciasNumerosTramiteAsync();
        Task<IEnumerable<object>> GetSugerenciasDropdownAsync();
        Task<IEnumerable<PasantiaDto>> BuscarAvanzadoAsync(PasantiaBusquedaAvanzadaDto filtro);
        Task<IEnumerable<PasantiaShowTableDto>> GetAllPasantiasShowTableAsync();
        Task<IEnumerable<PasantiaDto>> GetPasantiasPorVencerEnDiasAsync(int dias);
    }
}
