using System.Threading.Tasks;
using Backend.Interfaces;
using Backend.Models;
using Backend.Reports.Contrato_Pasantia_Estudiante;

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
            var estudiante = await _repoEstudiantes.GetByIdAsync((int)pasantia.IdEstudiante);
            if (estudiante == null)
                throw new System.Exception($"No se encontró el estudiante con id {pasantia.IdEstudiante}");

            // Obtener el convenio
            var convenio = await _repoConvenios.GetByIdAsync((int)pasantia.IdConvenio);
            if (convenio == null)
                throw new System.Exception($"No se encontró el convenio con id {pasantia.IdConvenio}");

            // Obtener la empresa
            var empresa = await _repoEmpresas.GetByIdAsync((int)convenio.IdEmpresa);
            if (empresa == null)
                throw new System.Exception($"No se encontró la empresa con id {convenio.IdEmpresa}");


            // Mapear a DTO plano para el reporte
            return ReportDataFactory.FromEntities(pasantia, estudiante, empresa, convenio);
        }
    }
}
