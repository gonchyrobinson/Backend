using Backend.DTOs.ConvenioDtos;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioConvenios : IService<Convenio, ConvenioDto, ConvenioUpdateDto, ConvenioCreateDto>
    {
        Task<IEnumerable<ConvenioEmpresaDto>> ListarConveniosConEmpresaAsync(ConvenioEmpresaFiltroDto filtro);
        Task<bool> AsignarEmpresaAsync(AsignarEmpresaDto dto);
        Task<bool> CaducarConvenioAsync(int convenioId, DateOnly? fechaCaducidad = null);
        Task<IEnumerable<EmpresaConvenioDropdownDto>> GetEmpresasConUltimoConvenioVigenteAsync(string? tipoAcuerdo = null);
        Task<IEnumerable<ConvenioEmpresaDto>> GetConveniosPorVencerEnDiasAsync(int dias);
    }
}
