using AutoMapper;
using Backend.DTOs.AuditoriaDtos;
using Backend.Interfaces.Repositories;
using Backend.Interfaces.Services;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioAuditoria : BaseService<Auditoria, AuditoriaDto, AuditoriaUpdateDto, AuditoriaDto>, IServicioAuditoria
    {
        private readonly IRepositorioAuditoria _repo;

        public ServicioAuditoria(IRepositorioAuditoria repo, IMapper mapper)
            : base(repo, mapper)
        {
            _repo = repo;
        }

        protected override int GetIdFromUpdateDto(AuditoriaUpdateDto dto)
        {
            return dto.IdAuditoria;
        }

        public async Task<IEnumerable<AuditoriaDto>> BuscarAsync(AuditoriaBuscarDto filtro)
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
