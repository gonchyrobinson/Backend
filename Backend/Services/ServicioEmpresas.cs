using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;

using System.Collections.Generic;
using System.Linq;

namespace Backend.Services
{
    public class ServicioEmpresas : BaseService<Empresa, EmpresaDto>
    {
        public ServicioEmpresas(RepositorioEmpresas repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<IEnumerable<EmpresaDto>> BuscarAvanzadoAsync(
            string? nombre,
            string? vigencia,
            string? tipoContrato,
            DateOnly? fechaInicioDesde,
            DateOnly? fechaInicioHasta,
            DateOnly? fechaFinDesde,
            DateOnly? fechaFinHasta)
        {
            var empresas = await _repository.GetAllAsync();
            var query = empresas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(e => !string.IsNullOrEmpty(e.Nombre) && e.Nombre.ToLower().Contains(nombre.ToLower()));
            if (!string.IsNullOrWhiteSpace(vigencia))
                query = query.Where(e => !string.IsNullOrEmpty(e.Vigencia) && e.Vigencia.ToLower() == vigencia.ToLower());
            if (!string.IsNullOrWhiteSpace(tipoContrato))
                query = query.Where(e => !string.IsNullOrEmpty(e.TipoContrato) && e.TipoContrato.ToLower() == tipoContrato.ToLower());
            if (fechaInicioDesde.HasValue)
                query = query.Where(e => e.FechaInicio >= fechaInicioDesde);
            if (fechaInicioHasta.HasValue)
                query = query.Where(e => e.FechaInicio <= fechaInicioHasta);
            if (fechaFinDesde.HasValue)
                query = query.Where(e => e.FechaFin >= fechaFinDesde);
            if (fechaFinHasta.HasValue)
                query = query.Where(e => e.FechaFin <= fechaFinHasta);

            return _mapper.Map<IEnumerable<EmpresaDto>>(query);
        }

        protected override int GetIdFromDto(EmpresaDto dto)
        {
            return dto.IdEmpresa;
        }
    }
}
