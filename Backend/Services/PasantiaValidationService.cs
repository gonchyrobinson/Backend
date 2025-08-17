using Backend.DTOs;
using Backend.Exceptions;

namespace Backend.Services
{
    public class PasantiaValidationService
    {
        private static readonly string[] ValoresFrecuencia = { "Mensual", "Trimestral", "Semestral", "Anual" };
        private static readonly string[] ValoresTipoAcuerdo = { "Pasantia", "PPS", "otro" };

        public void ValidateCreate(PasantiaCreateDto dto)
        {
            if (!string.IsNullOrEmpty(dto.FrecuenciaPago) && !ValoresFrecuencia.Contains(dto.FrecuenciaPago))
            {
                throw new ValidationException($"FrecuenciaPago debe ser uno de: {string.Join(", ", ValoresFrecuencia)}", "Pasantia");
            }
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !ValoresTipoAcuerdo.Contains(dto.TipoAcuerdo))
            {
                throw new ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", ValoresTipoAcuerdo)}", "Pasantia");
            }
        }
    }
}
