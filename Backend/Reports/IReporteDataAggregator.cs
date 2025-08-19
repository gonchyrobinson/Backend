namespace Backend.Reports
{
    // Interface genérica para obtener los datos de un reporte
    public interface IReporteDataAggregator<TRequest, TData>
    {
        Task<TData> GetDataAsync(TRequest request);
    }
}
