using iText.Forms;
using iText.Kernel.Pdf;


namespace Backend.Reports
{
    // Servicio base genérico para generación de reportes PDF

    public abstract class BasePdfReportService<TRequest, TData>
    {
        protected readonly IReporteDataAggregator<TRequest, TData> _aggregator;
        protected readonly IReporteRenderer<TData> _renderer;
        protected readonly string _templatePath;

        protected BasePdfReportService(
            IReporteDataAggregator<TRequest, TData> aggregator,
            IReporteRenderer<TData> renderer,
            string templatePath)
        {
            _aggregator = aggregator;
            _renderer = renderer;
            _templatePath = templatePath;
        }

        // Método principal: orquesta el flujo de generación del PDF
        public async Task<byte[]> GenerateReportAsync(TRequest request)
        {
            try
            {
                var data = await _aggregator.GetDataAsync(request);
                var fields = _renderer.MapFields(data);

                // --- Lógica de generación de PDF usando iText7 ---
                using var ms = new MemoryStream();
                using var pdfReader = new PdfReader(_templatePath);
                using var pdfWriter = new PdfWriter(ms);
                using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);

                var form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                var formFields = form.GetAllFormFields();

                foreach (var field in fields)
                {
                    if (formFields.ContainsKey(field.Key))
                        formFields[field.Key].SetValue(field.Value ?? "");
                }

                form.FlattenFields(); // Opcional: hace los campos no editables
                pdfDoc.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                // Puedes loguear el error aquí si tienes un logger
                throw new Exception($"Error al generar el PDF: {ex.Message}", ex);
            }
        }
    }
}

