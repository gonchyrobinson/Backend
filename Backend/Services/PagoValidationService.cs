using Backend.DTOs.PagosDtos;
using Backend.Exceptions;
using Backend.Interfaces.Repositories;
using Backend.Models;

namespace Backend.Services
{
    public class PagoValidationService
    {
        private readonly IRepository<Pasantia> _repoPasantias;
        public PagoValidationService(IRepository<Pasantia> repoPasantias)
        {
            _repoPasantias = repoPasantias;
        }

        public async Task ValidateCreateAsync(CreatePagosDto dto)
        {
            if (dto.IdPasantia != null)
            {
                var pasantia = await _repoPasantias.GetByIdAsync(dto.IdPasantia.Value);
                if (pasantia == null)
                    throw new ValidationException($"No existe una pasantía con ID {dto.IdPasantia}");
            }
        }

        public async Task ValidateUpdateAsync(PagosDto dto)
        {
            if (dto.IdPasantia != null)
            {
                var pasantia = await _repoPasantias.GetByIdAsync(dto.IdPasantia.Value);
                if (pasantia == null)
                    throw new ValidationException($"No existe una pasantía con ID {dto.IdPasantia}");
            }
        }
    }
}
