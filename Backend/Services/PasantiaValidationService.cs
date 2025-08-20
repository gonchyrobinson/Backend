using System.Text.RegularExpressions;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class PasantiaValidationService
    {
        // Constantes basadas en la Ley 26427 de Pasantías
        private static readonly string[] ValoresFrecuencia = { "Mensual", "Trimestral", "Semestral", "Anual" };
        private static readonly string[] ValoresTipoAcuerdo = { "Pasantia", "PPS", "otro" };
        private const int MAX_RENOVACIONES = 3;
        private const int MAX_HORAS_SEMANALES = 20;
        private const int DURACION_MAXIMA_MESES = 12;

        /// <summary>
        /// Valida los datos básicos para la creación de una pasantía
        /// </summary>
        public void ValidateCreate(PasantiaCreateDto dto)
        {
            ValidateBasicFields(
                dto.IdEstudiante, dto.IdConvenio, dto.FechaInicio, dto.FechaFin,
                dto.TutorEmpresa, dto.TutorFacultad, dto.DniTutorFacultad,
                dto.AsignacionMensual, dto.ObraSocial, dto.Art,
                dto.TipoAcuerdo, dto.FrecuenciaPago, dto.HorasSemanales
            );
        }

        /// <summary>
        /// Valida los datos básicos para la actualización de una pasantía (mismas validaciones que crear)
        /// </summary>
        public void ValidateUpdate(PasantiaDto dto)
        {
            ValidateBasicFields(
                dto.IdEstudiante, dto.IdConvenio, dto.FechaInicio, dto.FechaFin,
                dto.TutorEmpresa, dto.TutorFacultad, dto.DniTutorFacultad,
                dto.AsignacionMensual, dto.ObraSocial, dto.Art,
                dto.TipoAcuerdo, dto.FrecuenciaPago, dto.HorasSemanales
            );
        }

        public async Task ValidateDeleteAsync(int idPasantia, IRepositorioPagos repoPagos, IRepositorioPasantias repoPasantias)
        {
            var pasantia = await repoPasantias.GetByIdAsync(idPasantia);
            if (pasantia == null)
                throw new NotFoundException($"Pasantía con ID {idPasantia} no encontrada");
            
        }

        /// <summary>
        /// Valida todas las reglas de la Ley 26427 para la creación de una pasantía
        /// </summary>
        public async Task ValidateLegalRequirementsAsync(PasantiaCreateDto dto, IRepositorioPasantias repoPasantias, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios)
        {
            if (!dto.IdEstudiante.HasValue) return;
            
            await ValidateMaxRenovacionesAsync(dto.IdEstudiante.Value, repoPasantias);
            await ValidatePasantiasSimultaneasAsync(dto, repoPasantias);
            await ValidateTotalPasantiaDurationAsync(dto.IdEstudiante.Value, repoPasantias);
        }

        public async Task ValidateForeignKeysAsync(int? idEstudiante, int? idConvenio, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios)
        {
            if (idEstudiante.HasValue && idEstudiante.Value > 0)
            {
                var estudiante = await repoEstudiantes.GetByIdAsync(idEstudiante.Value);
                if (estudiante == null)
                    throw new NotFoundException($"Estudiante con ID {idEstudiante.Value} no encontrado");
            }
            
            if (idConvenio.HasValue && idConvenio.Value > 0)
            {
                var convenio = await repoConvenios.GetByIdAsync(idConvenio.Value);
                if (convenio == null)
                    throw new NotFoundException($"Convenio con ID {idConvenio.Value} no encontrado");
                ValidateConvenioVigente(convenio);
            }
        }

        public List<Pago> GenerarPagosAutomaticos(PasantiaCreateDto dto, int idPasantia)
        {
            var pagos = new List<Pago>();
            if (!dto.FechaInicio.HasValue || !dto.FechaFin.HasValue || string.IsNullOrEmpty(dto.FrecuenciaPago) || dto.AsignacionMensual <= 0)
                return pagos;

            var fechaActual = dto.FechaInicio.Value;
            var fechaFin = dto.FechaFin.Value;
            var monto = dto.AsignacionMensual * 0.05m;

            while (fechaActual < fechaFin)
            {
                pagos.Add(new Pago
                {
                    IdPasantia = idPasantia,
                    FechaVencimiento = fechaActual,
                    Monto = monto,
                    Pagado = false
                });

                fechaActual = dto.FrecuenciaPago switch
                {
                    "Mensual" => fechaActual.AddMonths(1),
                    "Trimestral" => fechaActual.AddMonths(3),
                    "Semestral" => fechaActual.AddMonths(6),
                    "Anual" => fechaActual.AddYears(1),
                    _ => throw new ValidationException("FrecuenciaPago inválida", "Pasantia")
                };
            }
            return pagos;
        }

        #region Private Validation Methods

        /// <summary>
        /// Método genérico para validar todos los campos básicos sin depender del tipo de DTO
        /// </summary>
        private void ValidateBasicFields(
            int? idEstudiante, int? idConvenio, DateOnly? fechaInicio, DateOnly? fechaFin,
            string? tutorEmpresa, string? tutorFacultad, string? DniTutorFacultad,
            decimal? asignacionMensual, string? obraSocial, string? art,
            string? tipoAcuerdo, string? frecuenciaPago, int? horasSemanales)
        {
            ValidateRequiredFields(idEstudiante, idConvenio, fechaInicio, fechaFin,
                tutorEmpresa, tutorFacultad, DniTutorFacultad, asignacionMensual, obraSocial, art);

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                ValidateDateRangeLogic(fechaInicio.Value, fechaFin.Value, tipoAcuerdo);
            }

            ValidateTipoAcuerdo(tipoAcuerdo);
            ValidateFrecuenciaPago(frecuenciaPago);
            ValidateHorasSemanales(horasSemanales);

            if (tipoAcuerdo == "Pasantia" && (!asignacionMensual.HasValue || asignacionMensual <= 0))
            {
                throw new ValidationException("Las pasantías deben ser remuneradas", "Pasantia");
            }

            if (tipoAcuerdo == "PPS" && asignacionMensual.HasValue && asignacionMensual > 0)
            {
                throw new ValidationException("Las PPS no pueden ser remuneradas", "Pasantia");
            }
        }

        private void ValidateRequiredFields(
            int? idEstudiante, int? idConvenio, DateOnly? fechaInicio, DateOnly? fechaFin,
            string? tutorEmpresa, string? tutorFacultad, string? DniTutorFacultad,
            decimal? asignacionMensual, string? obraSocial, string? art)
        {
            var missingFields = new List<string>();
            
            if (!idEstudiante.HasValue || idEstudiante <= 0) missingFields.Add("Estudiante");
            if (!idConvenio.HasValue || idConvenio <= 0) missingFields.Add("Convenio");
            if (!fechaInicio.HasValue) missingFields.Add("Fecha de inicio");
            if (!fechaFin.HasValue) missingFields.Add("Fecha de fin");
            if (string.IsNullOrWhiteSpace(tutorEmpresa)) missingFields.Add("Tutor de empresa");
            if (string.IsNullOrWhiteSpace(tutorFacultad)) missingFields.Add("Tutor de facultad");
            if (string.IsNullOrWhiteSpace(DniTutorFacultad)) missingFields.Add("DNI del tutor de facultad");
            if (!asignacionMensual.HasValue || asignacionMensual <= 0) missingFields.Add("Asignación mensual");
            if (string.IsNullOrWhiteSpace(obraSocial)) missingFields.Add("Obra social");
            if (string.IsNullOrWhiteSpace(art)) missingFields.Add("ART");
                
            if (missingFields.Count > 0)
                throw new ValidationException($"Campos obligatorios faltantes: {string.Join(", ", missingFields)}", "Pasantia");
        }
        
        private void ValidateDateRange(DateOnly? fechaInicio, DateOnly? fechaFin, string? tipoAcuerdo)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue) return;

            ValidateDateRangeLogic(fechaInicio.Value, fechaFin.Value, tipoAcuerdo);
        }

        private void ValidateTipoAcuerdo(string? tipoAcuerdo)
        {
            if (!string.IsNullOrEmpty(tipoAcuerdo) && !ValoresTipoAcuerdo.Contains(tipoAcuerdo))
                throw new ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", ValoresTipoAcuerdo)}", "Pasantia");
        }

        private void ValidateFrecuenciaPago(string? frecuenciaPago)
        {
            if (!string.IsNullOrEmpty(frecuenciaPago) && !ValoresFrecuencia.Contains(frecuenciaPago))
                throw new ValidationException($"FrecuenciaPago debe ser uno de: {string.Join(", ", ValoresFrecuencia)}", "Pasantia");
        }

        private void ValidateHorasSemanales(int? horasSemanales)
        {
            if (!horasSemanales.HasValue) return;
            
            if (horasSemanales <= 0)
                throw new ValidationException("Las horas semanales deben ser un valor positivo", "Pasantia");
            
            if (horasSemanales > MAX_HORAS_SEMANALES)
                throw new ValidationException($"Las horas semanales no pueden exceder de {MAX_HORAS_SEMANALES} según la Ley 26427", "Pasantia");
        }

        private void ValidateDateRangeLogic(DateOnly fechaInicio, DateOnly fechaFin, string? tipoAcuerdo)
        {
            if (fechaInicio >= fechaFin)
                throw new ValidationException("La fecha de inicio debe ser anterior a la fecha de fin", "Pasantia");

            if (fechaInicio < DateOnly.FromDateTime(DateTime.Today))
                throw new ValidationException("La fecha de inicio no puede ser anterior a la fecha actual", "Pasantia");

            var mesesDuracion = ((fechaFin.Year - fechaInicio.Year) * 12) + (fechaFin.Month - fechaInicio.Month);

            if (tipoAcuerdo == "Pasantia")
            {
                if (mesesDuracion < 2)
                    throw new ValidationException("La duración mínima de una pasantía debe ser de 2 meses", "Pasantia");

                if (mesesDuracion > 12)
                    throw new ValidationException("La duración máxima de una pasantía no puede superar los 12 meses", "Pasantia");
            }
            else if (tipoAcuerdo == "PPS")
            {
                if (mesesDuracion > 4)
                    throw new ValidationException("La duración máxima de una PPS no puede superar los 4 meses", "Pasantia");
            }
        }

        private void ValidateDniFormat(string? dni)
        {
            if (string.IsNullOrEmpty(dni))
                throw new ValidationException("El DNI del estudiante no puede estar vacío", "Estudiante");
                
            if (!Regex.IsMatch(dni, @"^\d{7,8}$"))
                throw new ValidationException("El formato del DNI debe ser de 7 u 8 dígitos sin puntos ni espacios", "Estudiante");
        }
        
        private void ValidateConvenioVigente(Convenio convenio)
        {
            if (convenio.FechaCaducidad.HasValue && convenio.FechaCaducidad < DateOnly.FromDateTime(DateTime.Today))
                throw new ValidationException($"El convenio ha caducado el {convenio.FechaCaducidad}. No se pueden crear pasantías con convenios vencidos.", "Convenio");
        }

        private async Task ValidateMaxRenovacionesAsync(int idEstudiante, IRepositorioPasantias repoPasantias)
        {
            var pasantiasEstudiante = await repoPasantias.GetByEstudianteIdAsync(idEstudiante);
            if (pasantiasEstudiante.Count() >= MAX_RENOVACIONES)
                throw new ValidationException($"El estudiante ya ha alcanzado el máximo de {MAX_RENOVACIONES} pasantías/renovaciones permitidas por la Ley 26427", "Pasantia");
        }
        
        private async Task ValidatePasantiasSimultaneasAsync(PasantiaCreateDto dto, IRepositorioPasantias repoPasantias)
        {
            if (!dto.IdEstudiante.HasValue || !dto.FechaInicio.HasValue || !dto.FechaFin.HasValue)
                return;
                
            var pasantiasEstudiante = await repoPasantias.GetByEstudianteIdAsync(dto.IdEstudiante.Value);
            
            foreach (var pasantia in pasantiasEstudiante.Where(p => p.FechaInicio.HasValue && p.FechaFin.HasValue))
            {
                bool hayOverlap = dto.FechaInicio <= pasantia.FechaFin && dto.FechaFin >= pasantia.FechaInicio;
                if (hayOverlap)
                    throw new ValidationException("El estudiante ya tiene una pasantía activa durante el período propuesto. No se permiten pasantías simultáneas según la Ley 26427", "Pasantia");
            }
        }

        private async Task ValidateTotalPasantiaDurationAsync(int idEstudiante, IRepositorioPasantias repoPasantias)
        {
            var pasantiasEstudiante = await repoPasantias.GetByEstudianteIdAsync(idEstudiante);

            var totalMeses = pasantiasEstudiante
                .Where(p => p.FechaInicio.HasValue && p.FechaFin.HasValue)
                .Sum(p => ((p.FechaFin!.Value.Year - p.FechaInicio!.Value.Year) * 12) + (p.FechaFin.Value.Month - p.FechaInicio.Value.Month));

            if (totalMeses > 18)
            {
                throw new ValidationException($"El estudiante ya ha acumulado un total de {totalMeses} meses de pasantías, excediendo el máximo permitido de 18 meses.", "Pasantia");
            }
        }

        #endregion
    }
}
