using System.ComponentModel.DataAnnotations;

namespace Backend.DTOValidations
{
    public static class CommonValidations
    {
        public static readonly HashSet<string> CarrerasValidas = new HashSet<string>
        {
            "AGRIMENSURA",
            "INGENIERÍA AZUCARERA",
            "INGENIERÍA BIOMÉDICA",
            "INGENIERÍA CIVIL",
            "INGENIERÍA EN COMPUTACIÓN",
            "INGENIERÍA EN INFORMÁTICA",
            "INGENIERÍA ELÉCTRICA",
            "INGENIERÍA ELECTRÓNICA",
            "INGENIERÍA GEODÉSICA Y GEOFÍSICA",
            "INGENIERÍA INDUSTRIAL",
            "INGENIERÍA MECÁNICA",
            "INGENIERÍA QUÍMICA",
            "LICENCIATURA EN FÍSICA",
            "LICENCIATURA EN MATEMÁTICA",
            "LICENCIATURA EN INFORMÁTICA",
            "DISEÑO DE ILUMINACIÓN",
            "PROGRAMADOR UNIVERSITARIO",
            "TECNICATURA UNIVERSITARIA EN TECNOLOGÍA",
            "AZUCARERA E INDUSTRIAS DERIVADAS",
            "TECNICATURA UNIVERSITARIA EN FÍSICA",
            "TECNICATURA UNIVERSITARIA EN FÍSICA AMBIENTAL",
            "OTRA"
        };

        public static readonly HashSet<string> TiposAcuerdoValidos = new HashSet<string>
        {
            "Pasantia", "PPS", "otro"
        };

        public static readonly HashSet<string> FrecuenciasPagoValidas = new HashSet<string>
        {
            "Mensual", "Trimestral", "Semestral", "Anual"
        };

        public static readonly HashSet<string> TiposContratoValidos = new HashSet<string>
        {
            "PPS", "Pasantia", "otro"
        };

        public static readonly HashSet<string> EstadosValidos = new HashSet<string>
        {
            "vigente", "no_vigente"
        };

        public static bool EsDniValido(string? documento)
        {
            if (string.IsNullOrWhiteSpace(documento)) return false;
            return documento.All(char.IsDigit) && (documento.Length == 7 || documento.Length == 8);
        }

        public static bool EsCarreraValida(string? carrera)
        {
            if (string.IsNullOrWhiteSpace(carrera)) return true; // Opcional
            return CarrerasValidas.Contains(carrera);
        }

        public static bool EsTipoAcuerdoValido(string? tipoAcuerdo)
        {
            if (string.IsNullOrWhiteSpace(tipoAcuerdo)) return false;
            return TiposAcuerdoValidos.Contains(tipoAcuerdo);
        }

        public static bool EsFrecuenciaPagoValida(string? frecuencia)
        {
            if (string.IsNullOrWhiteSpace(frecuencia)) return false;
            return FrecuenciasPagoValidas.Contains(frecuencia);
        }

        public static bool EsTipoContratoValido(string? tipoContrato)
        {
            if (string.IsNullOrWhiteSpace(tipoContrato)) return false;
            return TiposContratoValidos.Contains(tipoContrato);
        }

        public static bool EsMontoValido(decimal? monto)
        {
            return monto.HasValue && monto.Value >= 0;
        }

        public static bool EsFechaValida(DateOnly? fecha)
        {
            return fecha.HasValue && fecha.Value >= DateOnly.FromDateTime(DateTime.Now.AddYears(-100));
        }

        public static bool EsRangoFechasValido(DateOnly? fechaInicio, DateOnly? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue) return false;
            return fechaInicio.Value <= fechaFin.Value;
        }

        public static bool EsVigenciaValida(string? vigencia)
        {
            if (string.IsNullOrWhiteSpace(vigencia)) return true; // Opcional
            return EstadosValidos.Contains(vigencia);
        }
    }
}
