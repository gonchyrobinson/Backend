using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioEmpresaFiltroDto : IValidatableObject
    {
        public DateOnly? FechaFirmaDesde { get; set; }
        public DateOnly? FechaFirmaHasta { get; set; }
        public DateOnly? FechaCaducidadDesde { get; set; }
        public DateOnly? FechaCaducidadHasta { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public string? Carrera { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateConvenioEmpresaFiltro(
                FechaFirmaDesde, FechaFirmaHasta, FechaCaducidadDesde, FechaCaducidadHasta,
                NombreEmpresa, DocRepresentanteFacultad, Carrera);
        }
    }
}
