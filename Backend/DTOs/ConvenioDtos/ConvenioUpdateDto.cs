using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioUpdateDto : IValidatableObject
    {
        public int IdConvenio { get; set; }
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public int? NroAcuerdoMarco { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DomicilioAlternativo { get; set; }
        public string? DocRepresentanteEmpresa { get; set; }
        public string? RepresentanteFacultad { get; set; }
        public string? DocRepresentanteFacultad { get; set; }
        public DateOnly? FechaFirma { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public string? Caracter { get; set; }
        public string? Sudocu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateConvenio(
                IdEmpresa, RepresentanteEmpresa, DocRepresentanteEmpresa,
                RepresentanteFacultad, DocRepresentanteFacultad,
                FechaFirma, FechaCaducidad, DomicilioLegal, DomicilioAlternativo, isCreate: false);
        }
    }
}
