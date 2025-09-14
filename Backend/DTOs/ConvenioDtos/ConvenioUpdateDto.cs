using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.ConvenioDtos
{
    public class ConvenioUpdateDto : IValidatableObject
    {
        public int IdConvenio { get; set; }
        public int? IdEmpresa { get; set; }
        public string? RepresentanteEmpresa { get; set; }
        public string? DomicilioLegal { get; set; }
        public string? DocRepresentanteEmpresa { get; set; }
        public string? NombreDecano { get; set; }
        public string? DocumentoDecano { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
        public string? TipoAcuerdo { get; set; }
        public string? ExpedienteSudocu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ConvenioDtoValidator.ValidateConvenio(
                IdEmpresa, RepresentanteEmpresa, DocRepresentanteEmpresa,
                NombreDecano, DocumentoDecano,
                FechaInicio, FechaCaducidad, DomicilioLegal, TipoAcuerdo, isCreate: false);
        }
    }
}
