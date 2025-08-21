using Backend.DTOs;
using Backend.Exceptions;
using Backend.Interfaces.Repositories;
using Backend.DTOs.ConvenioDtos;

namespace Backend.Services
{
    public class EmpresaValidationService
    {
        private readonly IRepositorioConvenios _repoConvenios;
        public EmpresaValidationService(IRepositorioConvenios repoConvenios)
        {
            _repoConvenios = repoConvenios;
        }

        public async Task ValidateDeleteAsync(int idEmpresa)
        {
            var convenios = await _repoConvenios.ListarConveniosConEmpresa(new ConvenioEmpresaFiltroDto());
            if (convenios.Any(c => c.IdEmpresa == idEmpresa))
            {
                throw new ValidationException("No se puede eliminar la empresa porque tiene convenios asociados", "Empresa");
            }
        }
    }
}
