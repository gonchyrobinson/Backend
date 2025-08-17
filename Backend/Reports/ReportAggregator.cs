using Backend.Interfaces;

namespace Backend.Reports
{
    public class ReportAggregator : IReporteDataAggregator<int, ReportData>
    {
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioEmpresas _repoEmpresas;
        private readonly IRepositorioConvenios _repoConvenios;

        public ReportAggregator(
            IRepositorioPasantias repoPasantias,
            IRepositorioEstudiantes repoEstudiantes,
            IRepositorioEmpresas repoEmpresas,
            IRepositorioConvenios repoConvenios)
        {
            _repoPasantias = repoPasantias;
            _repoEstudiantes = repoEstudiantes;
            _repoEmpresas = repoEmpresas;
            _repoConvenios = repoConvenios;
        }

        public async Task<ReportData> GetDataAsync(int id)
        {
            // Obtener la pasantía
            var pasantia = await _repoPasantias.GetByIdAsync(id);
            if (pasantia == null)
                throw new System.Exception($"No se encontró la pasantía con id {id}");

            // Obtener el estudiante
            var idEstudiante = pasantia.IdEstudiante ?? throw new System.Exception($"La pasantía {id} no tiene IdEstudiante asociado");
            var estudiante = await _repoEstudiantes.GetByIdAsync(idEstudiante);
            if (estudiante == null)
                throw new System.Exception($"No se encontró el estudiante con id {idEstudiante}");

            // Obtener el convenio
            var idConvenio = pasantia.IdConvenio ?? throw new System.Exception($"La pasantía {id} no tiene IdConvenio asociado");
            var convenio = await _repoConvenios.GetByIdAsync(idConvenio);
            if (convenio == null)
                throw new System.Exception($"No se encontró el convenio con id {idConvenio}");

            // Obtener la empresa
            var idEmpresa = convenio.IdEmpresa ?? throw new System.Exception($"El convenio {idConvenio} no tiene IdEmpresa asociado");
            var empresa = await _repoEmpresas.GetByIdAsync(idEmpresa);
            if (empresa == null)
                throw new System.Exception($"No se encontró la empresa con id {idEmpresa}");


            // Mapear a DTO plano para el reporte
            return ReportDataFactory.FromEntities(pasantia, estudiante, empresa, convenio);
        }
    }
}
