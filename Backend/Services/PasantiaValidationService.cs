using Backend.DTOs;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

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

        public async Task ValidateDeleteAsync(int idPasantia, IRepositorioPagos repoPagos, IRepositorioPasantias repoPasantias)
        {
            var pasantia = await repoPasantias.GetByIdAsync(idPasantia);
            if (pasantia == null)
            {
                throw new NotFoundException($"Pasantía con ID {idPasantia} no encontrada");
            }
            var pagos = await repoPagos.GetByPasantiaIdAsync(idPasantia);
            if (pagos != null && pagos.Any())
            {
                throw new ValidationException("No se puede eliminar la pasantía porque tiene pagos asociados.", "Pasantia");
            }
        }

        public async Task ValidateForeignKeysAsync(PasantiaCreateDto dto, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios)
        {
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }
        }

        public List<Pago> GenerarPagosAutomaticos(PasantiaCreateDto dto, int idPasantia)
        {
            var pagos = new List<Pago>();
            if (dto.FechaInicio.HasValue && dto.FechaFin.HasValue && !string.IsNullOrEmpty(dto.FrecuenciaPago) && (dto.MontoPago > 0))
            {
                var fechaInicio = dto.FechaInicio.Value;
                var fechaFin = dto.FechaFin.Value;
                var frecuencia = dto.FrecuenciaPago;
                var monto = dto.MontoPago * 0.05m;
                var fechaActual = fechaInicio;
                while (fechaActual < fechaFin)
                {
                    pagos.Add(new Pago
                    {
                        IdPasantia = idPasantia,
                        FechaVencimiento = fechaActual,
                        Monto = monto,
                        Pagado = false,
                        FechaPago = null,
                        Observaciones = null
                    });
                    switch (frecuencia)
                    {
                        case "Mensual": fechaActual = fechaActual.AddMonths(1); break;
                        case "Trimestral": fechaActual = fechaActual.AddMonths(3); break;
                        case "Semestral": fechaActual = fechaActual.AddMonths(6); break;
                        case "Anual": fechaActual = fechaActual.AddYears(1); break;
                        default: throw new ValidationException("FrecuenciaPago inválida", "Pasantia");
                    }
                }
            }
            return pagos;
        }
    }
}
