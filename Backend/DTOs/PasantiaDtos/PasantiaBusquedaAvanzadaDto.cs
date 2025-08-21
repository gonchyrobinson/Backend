using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaBusquedaAvanzadaDto : IValidatableObject
    {
        public string? Tramite { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? TipoAcuerdo { get; set; }
        public DateOnly? FechaInicioDesde { get; set; }
        public DateOnly? FechaInicioHasta { get; set; }
        public DateOnly? FechaFinDesde { get; set; }
        public DateOnly? FechaFinHasta { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public string? Estado { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return PasantiaDtoValidator.ValidatePasantiaBusqueda(
                Tramite, ObraSocial, Art, TutorEmpresa, TutorFacultad, TipoAcuerdo,
                FechaInicioDesde, FechaInicioHasta, FechaFinDesde, FechaFinHasta,
                IdEstudiante, IdConvenio, Estado);
        }
    }
}
