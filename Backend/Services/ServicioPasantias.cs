using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioPasantias : BaseService<Pasantia, PasantiaDto, PasantiaCreateDto>
    {
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IRepositorioEstudiantes _repoEstudiantes;
        private readonly IRepositorioConvenios _repoConvenios;

        public ServicioPasantias(IRepositorioPasantias repoPasantias, IRepositorioEstudiantes repoEstudiantes, IRepositorioConvenios repoConvenios, IMapper mapper)
            : base(repoPasantias, mapper)
        {
            _repoPasantias = repoPasantias;
            _repoEstudiantes = repoEstudiantes;
            _repoConvenios = repoConvenios;
        }

        protected override int GetIdFromDto(PasantiaDto dto)
        {
            return dto.IdPasantia;
        }

        // Métodos específicos para pasantías pueden agregarse aquí
        public async Task<IEnumerable<PasantiaDetalleDto>> GetAllDetalleAsync()
        {
            return await _repoPasantias.GetAllDetalleAsync();
        }


        public async Task<IEnumerable<PasantiaDto>> GetByConvenioIdAsync(int convenioId)
        {
            var entities = await _repoPasantias.GetByConvenioIdAsync(convenioId);
            if (entities == null || !entities.Any())
                throw new Backend.Exceptions.NotFoundException($"No se encontraron pasantias para el convenio con ID {convenioId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }

        public async Task<IEnumerable<PasantiaDto>> GetByEstudianteIdAsync(int estudianteId)
        {
            var entities = await _repoPasantias.GetByEstudianteIdAsync(estudianteId);
            if (entities == null || !entities.Any())
                throw new Backend.Exceptions.NotFoundException($"No se encontraron pasantias para el estudiante con ID {estudianteId}");
            return _mapper.Map<IEnumerable<PasantiaDto>>(entities);
        }
        public override async Task<PasantiaDto> CreateAsync(PasantiaCreateDto dto)
        {
            // Validar ENUM tipo_acuerdo
            var valoresValidos = new[] { "Pasantia", "PPS", "otro" };
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !valoresValidos.Contains(dto.TipoAcuerdo))
            {
                throw new Backend.Exceptions.ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", valoresValidos)}", "Pasantia");
            }
            // Validar claves foráneas
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await _repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new Backend.Exceptions.NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await _repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new Backend.Exceptions.NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }

            // Crear la pasantía
            var pasantiaDto = await base.CreateAsync(dto);

            // Lógica para crear los pagos automáticos
            if (dto.FechaInicio.HasValue && dto.FechaFin.HasValue && !string.IsNullOrEmpty(dto.FrecuenciaPago) && (dto.MontoPago > 0))
            {
                var fechaInicio = dto.FechaInicio.Value;
                var fechaFin = dto.FechaFin.Value;
                var frecuencia = dto.FrecuenciaPago;
                var monto = dto.MontoPago * 0.05m; //La empresa debe pagar el 5% de lo que le paga al estudiante

                var pagos = new List<Pago>();
                var fechaActual = fechaInicio;

                while (fechaActual < fechaFin)
                {
                    pagos.Add(new Pago
                    {
                        IdPasantia = pasantiaDto.IdPasantia,
                        FechaVencimiento = fechaActual,
                        Monto = monto,
                        Pagado = false,
                        FechaPago = null,
                        Observaciones = null
                    });

                    // Avanzar según la frecuencia
                    switch (frecuencia)
                    {
                        case "Mensual":
                            fechaActual = fechaActual.AddMonths(1);
                            break;
                        case "Trimestral":
                            fechaActual = fechaActual.AddMonths(3);
                            break;
                        case "Semestral":
                            fechaActual = fechaActual.AddMonths(6);
                            break;
                        case "Anual":
                            fechaActual = fechaActual.AddYears(1);
                            break;
                        default:
                            throw new Backend.Exceptions.ValidationException("FrecuenciaPago inválida", "Pasantia");
                    }
                }

                // Guardar los pagos en la base de datos
                foreach (var pago in pagos)
                {
                    await _repoPasantias.AgregarPagoAsync(pago); // Implementa este método en tu repositorio
                }
            }

            return pasantiaDto;
        }

        public override async Task<PasantiaDto> UpdateAsync(PasantiaDto dto)
        {
            var valoresValidos = new[] { "Pasantia", "PPS", "otro" };
            if (!string.IsNullOrEmpty(dto.TipoAcuerdo) && !valoresValidos.Contains(dto.TipoAcuerdo))
            {
                throw new Backend.Exceptions.ValidationException($"TipoAcuerdo debe ser uno de: {string.Join(", ", valoresValidos)}", "Pasantia");
            }
            if (dto.IdEstudiante.HasValue && dto.IdEstudiante.Value > 0)
            {
                var estudiante = await _repoEstudiantes.GetByIdAsync(dto.IdEstudiante.Value);
                if (estudiante == null)
                    throw new Backend.Exceptions.NotFoundException($"Estudiante con ID {dto.IdEstudiante.Value} no encontrado");
            }
            if (dto.IdConvenio.HasValue && dto.IdConvenio.Value > 0)
            {
                var convenio = await _repoConvenios.GetByIdAsync(dto.IdConvenio.Value);
                if (convenio == null)
                    throw new Backend.Exceptions.NotFoundException($"Convenio con ID {dto.IdConvenio.Value} no encontrado");
            }
            return await base.UpdateAsync(dto);
        }
    }
}
