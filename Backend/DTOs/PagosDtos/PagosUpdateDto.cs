using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PagosDtos
{
    public class PagosUpdateDto : IValidatableObject
    {
        public int IdPago { get; set; }
        public int? IdPasantia { get; set; }
        public bool? Pagado { get; set; }
        public DateOnly? FechaPago { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
        public decimal? Monto { get; set; }
        public string? Observaciones { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return PagoDtoValidator.ValidatePago(IdPasantia, Monto, FechaVencimiento, FechaPago, isCreate: false);
        }
    }
}
