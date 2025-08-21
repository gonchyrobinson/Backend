using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PasantiaDtos
{
    public class PasantiaUpdateDto : IValidatableObject
    {
        public int IdPasantia { get; set; }
        public int? IdEstudiante { get; set; }
        public int? IdConvenio { get; set; }
        public decimal? AsignacionMensual { get; set; }
        public string? ObraSocial { get; set; }
        public string? Art { get; set; }
        public string? TutorEmpresa { get; set; }
        public string? TutorFacultad { get; set; }
        public string? DniTutorFacultad { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? FrecuenciaPago { get; set; }
        public string? Observaciones { get; set; }
        public string? Sudocu { get; set; }
        public int? HorasSemanales { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return PasantiaDtoValidator.ValidatePasantia(
                IdEstudiante, IdConvenio, FechaInicio, FechaFin,
                TutorEmpresa, TutorFacultad, DniTutorFacultad,
                AsignacionMensual, ObraSocial, Art,
                TipoAcuerdo, FrecuenciaPago, HorasSemanales, isCreate: false);
        }
    }
}
