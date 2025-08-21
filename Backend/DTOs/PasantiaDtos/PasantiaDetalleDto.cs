using Backend.DTOs.ConvenioDtos;
using Backend.DTOs.StudentDtos;

namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaDetalleDto
    {
        public PasantiaDto Pasantia { get; set; } = null!;
        public StudentDto? Estudiante { get; set; }
        public ConvenioDto? Convenio { get; set; }
    }
}
