using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class EmpresaDto : IValidatableObject
    {
        public int IdEmpresa { get; set; }
        public string? Nombre { get; set; }
        public string? Vigencia
        {
            get
            {
                if (FechaFin.HasValue && FechaFin.Value < DateOnly.FromDateTime(DateTime.Today))
                    return "no_vigente";
                return "vigente";
            }
            set { /* setter requerido por serialización, pero ignorado */ }
        }

        [DataType(DataType.Date, ErrorMessage = "FechaInicio debe ser una fecha válida.")]
        public DateOnly? FechaInicio { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFin debe ser una fecha válida.")]
        public DateOnly? FechaFin { get; set; }

        [Required]
        [RegularExpression("^(temporal|indefinido|otro)$", ErrorMessage = "TipoContrato debe ser 'temporal', 'indefinido' u 'otro'.")]
        public string? TipoContrato { get; set; }

        public string? Encargado { get; set; }

        [Phone(ErrorMessage = "Celular debe tener formato de número de teléfono válido.")]
        public string? Celular { get; set; }

        [EmailAddress(ErrorMessage = "CorreoElectronico debe tener formato de correo electrónico válido.")]
        public string? CorreoElectronico { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Sudocu debe ser una fecha válida.")]
        public DateOnly? Sudocu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaInicio.HasValue && FechaFin.HasValue && FechaInicio.Value > FechaFin.Value)
            {
                yield return new ValidationResult("FechaInicio debe ser menor o igual a FechaFin.", new[] { nameof(FechaInicio), nameof(FechaFin) });
            }
        }
    }

    public class EmpresaCreateDto: IValidatableObject
    {
        public string? Nombre { get; set; }

        [RegularExpression("^(vigente|no_vigente)?$", ErrorMessage = "Vigencia debe ser 'vigente', 'no_vigente' o null.")]
        public string? Vigencia { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicio debe ser una fecha válida.")]
        public DateOnly? FechaInicio { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFin debe ser una fecha válida.")]
        public DateOnly? FechaFin { get; set; }

        [Required]
        [RegularExpression("^(temporal|indefinido|otro)$", ErrorMessage = "TipoContrato debe ser 'temporal', 'indefinido' u 'otro'.")]
        public string? TipoContrato { get; set; }

        public string? Encargado { get; set; }

        [Phone(ErrorMessage = "Celular debe tener formato de número de teléfono válido.")]
        public string? Celular { get; set; }

        [EmailAddress(ErrorMessage = "CorreoElectronico debe tener formato de correo electrónico válido.")]
        public string? CorreoElectronico { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Sudocu debe ser una fecha válida.")]
        public DateOnly? Sudocu { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaInicio.HasValue && FechaFin.HasValue && FechaInicio.Value > FechaFin.Value)
            {
                yield return new ValidationResult("FechaInicio debe ser menor o igual a FechaFin.", new[] { nameof(FechaInicio), nameof(FechaFin) });
            }
        }
    }
    public class EmpresaBusquedaAvanzadaDto : IValidatableObject
    {
        public string? Nombre { get; set; }

        public bool? Vigencia { get; set; }

        [RegularExpression("^(temporal|indefinido|otro)?$", ErrorMessage = "TipoContrato debe ser 'temporal', 'indefinido', 'otro' o null.")]
        public string? TipoContrato { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicioDesde debe ser una fecha válida.")]
        public DateOnly? FechaInicioDesde { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaInicioHasta debe ser una fecha válida.")]
        public DateOnly? FechaInicioHasta { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFinDesde debe ser una fecha válida.")]
        public DateOnly? FechaFinDesde { get; set; }

        [DataType(DataType.Date, ErrorMessage = "FechaFinHasta debe ser una fecha válida.")]
        public DateOnly? FechaFinHasta { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaInicioDesde.HasValue && FechaInicioHasta.HasValue && FechaInicioDesde.Value > FechaInicioHasta.Value)
            {
                yield return new ValidationResult("FechaInicioDesde debe ser menor o igual a FechaInicioHasta.", new[] { nameof(FechaInicioDesde), nameof(FechaInicioHasta) });
            }
            if (FechaFinDesde.HasValue && FechaFinHasta.HasValue && FechaFinDesde.Value > FechaFinHasta.Value)
            {
                yield return new ValidationResult("FechaFinDesde debe ser menor o igual a FechaFinHasta.", new[] { nameof(FechaFinDesde), nameof(FechaFinHasta) });
            }
        }
    }
}
