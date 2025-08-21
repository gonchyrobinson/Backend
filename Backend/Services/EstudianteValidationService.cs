using Backend.Exceptions;
using Backend.Interfaces.Repositories;

namespace Backend.Services
{
    public class EstudianteValidationService
    {
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IRepositorioEstudiantes _repoEstudiantes;

        public EstudianteValidationService(IRepositorioEstudiantes repoEstudiantes, IRepositorioPasantias repoPasantias)
        {
            _repoEstudiantes = repoEstudiantes;
            _repoPasantias = repoPasantias;
        }

        public async Task ValidateDeleteAsync(int idEstudiante)
        {
            var estudiante = await _repoEstudiantes.GetByIdAsync(idEstudiante);
            if (estudiante == null)
                throw new NotFoundException($"Estudiante con ID {idEstudiante} no encontrado");
            var pasantias = await _repoPasantias.GetByEstudianteIdAsync(idEstudiante);
            if (pasantias.Any())
            {
                throw new ValidationException($"No se puede eliminar el estudiante porque tiene pasantías asociadas.", "Estudiante");
            }
        }
    }
}
