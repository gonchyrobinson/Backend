using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public class ServicioEmpresas : BaseService<Empresa, EmpresaDto, EmpresaCreateDto>
    {
        private readonly IRepositorioEmpresas _repoEmpresas;

        public ServicioEmpresas(IRepositorioEmpresas repository, IMapper mapper) : base(repository, mapper)
        {
            _repoEmpresas = repository;
        }

        protected override int GetIdFromDto(EmpresaDto dto)
        {
            return dto.IdEmpresa;
        }
        public async Task<IEnumerable<EmpresaDto>> BuscarAvanzadoAsync(EmpresaBusquedaAvanzadaDto filtro)
        {
            var empresas = await _repoEmpresas.BuscarAvanzadoAsync(filtro);
            return _mapper.Map<IEnumerable<EmpresaDto>>(empresas);
        }
    }
}
