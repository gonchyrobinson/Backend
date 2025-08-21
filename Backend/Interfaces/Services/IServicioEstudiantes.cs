using Backend.DTOs.StudentDtos;
using Backend.Models;

namespace Backend.Interfaces.Services
{
    public interface IServicioEstudiantes : IService<Estudiante, StudentDto, StudentUpdateDto, StudentCreateDto>
    {
        Task<IEnumerable<StudentDto>> BuscarAvanzadoAsync(StudentBusquedaAvanzadaDto filtro);
        Task<IEnumerable<string>> GetSugerenciasNombresAsync();
        Task<IEnumerable<string>> GetSugerenciasApellidosAsync();
        Task<IEnumerable<object>> GetDocumentosUnicos();
    }
}
