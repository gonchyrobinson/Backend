using Backend.Exceptions;
using Backend.Interfaces.Repositories;

namespace Backend.Services
{
    public class ConvenioValidationService
    {
        private readonly IRepositorioConvenios _repoConvenios;
        private readonly IRepositorioPasantias _repoPasantias;
        public ConvenioValidationService(IRepositorioConvenios repoConvenios, IRepositorioPasantias repoPasantias)
        {
            _repoConvenios = repoConvenios;
            _repoPasantias = repoPasantias;
        }

        public async Task ValidateDeleteAsync(int idConvenio)
        {
            var convenio = await _repoConvenios.GetByIdAsync(idConvenio);
            if (convenio == null)
                throw new NotFoundException($"Convenio con ID {idConvenio} no encontrado");
            var pasantias = await _repoPasantias.GetByConvenioIdAsync(idConvenio);
            if (pasantias.Any())
            {
                throw new ValidationException($"No se puede eliminar el convenio porque tiene pasantías asociadas.", "Convenio");
            }
        }
    }
}
