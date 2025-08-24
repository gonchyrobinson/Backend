using System.ComponentModel.DataAnnotations;
using Backend.DTOValidations;

namespace Backend.DTOs.PagosDtos
{
    public class PagosBusquedaAvanzadaDto : IValidatableObject
    {
        public int? IdEmpresa { get; set; }
        public string? Estudiante { get; set; }
        public bool? EstadoPago { get; set; }
        public string? FechaVencimiento { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // No se requieren validaciones específicas para estos campos de búsqueda
            // Los campos son opcionales y se validan en el repositorio
            yield break;
        }
    }
}
