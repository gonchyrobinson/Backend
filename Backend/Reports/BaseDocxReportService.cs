using TemplateEngine.Docx;

namespace Backend.Reports
{
    // Servicio base genérico para generación de reportes DOCX
    public abstract class BaseDocxReportService<TRequest, TData>
    {
        protected readonly IReporteDataAggregator<TRequest, TData> _aggregator;
        protected readonly IReporteRenderer<TData> _renderer;
        protected readonly string _templatePath;

        protected BaseDocxReportService(
            IReporteDataAggregator<TRequest, TData> aggregator,
            IReporteRenderer<TData> renderer,
            string templatePath)
        {
            _aggregator = aggregator;
            _renderer = renderer;
            _templatePath = templatePath;
        }

        // Método principal: orquesta el flujo de generación del DOCX
        public async Task<byte[]> GenerateReportAsync(TRequest request)
        {
            var data = await _aggregator.GetDataAsync(request);
            var fields = _renderer.MapFields(data);

            using var ms = new MemoryStream();
            using (var templateStream = File.OpenRead(_templatePath))
            {
                templateStream.CopyTo(ms);
            }
            ms.Position = 0;

            using (var output = new MemoryStream())
            {
                ms.Position = 0;
                using (var doc = new TemplateProcessor(ms).SetRemoveContentControls(true))
                {
                    var values = new Content();
                    foreach (var field in fields)
                    {
                        values.Fields.Add(new FieldContent(field.Key, field.Value ?? string.Empty));
                    }
                    doc.FillContent(values);
                    doc.SaveChanges();
                }
                ms.Position = 0;
                ms.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}
