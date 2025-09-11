namespace Backend.Constants
{
    public static class AppConstants
    {
        // Mantenemos solo la constante que se utiliza
        public const string CorsPolicyName = "AllowReactApp";

        // Valores permitidos para Convenio.TipoAcuerdo (sin afectar valores históricos distintos)
        public static readonly string[] ConvenioTipoAcuerdoPermitidos = new[]
        {
            "Carta Acuerdo de Cooperación y Asistencia Técnica",
            "Pasantías y PPS",
            "otro"
        };
    }
}