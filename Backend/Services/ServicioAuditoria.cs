using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class ServicioAuditoria
    {
        private readonly IRepositorioAuditoria _repo;
        private readonly IMapper _mapper;

        public ServicioAuditoria(IRepositorioAuditoria repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuditoriaDto>> BuscarAsync(Backend.DTOs.AuditoriaBuscarDto filtro)
        {
            var logs = await _repo.BuscarAsync(filtro);
            return _mapper.Map<IEnumerable<AuditoriaDto>>(logs);
        }

        public async Task RegistrarAsync(AuditoriaDto dto)
        {
            var entity = _mapper.Map<Auditoria>(dto);
            await _repo.AddAsync(entity);
        }
    }
}
