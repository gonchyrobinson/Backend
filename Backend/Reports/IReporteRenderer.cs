namespace Backend.Reports
{
    // Interface genérica para mapear los datos a los fields del PDF
    public interface IReporteRenderer<TData>
    {
        // Devuelve un diccionario fieldName => valor para el template PDF
        Dictionary<string, string> MapFields(TData data);
    }
}
