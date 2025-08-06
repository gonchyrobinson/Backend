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
        private readonly IRepositorioConvenios _repoConvenios;

        public ServicioEmpresas(IRepositorioEmpresas repoEmpresas, IRepositorioConvenios repoConvenios, IMapper mapper)
            : base(repoEmpresas, mapper)
        {
            _repoEmpresas = repoEmpresas;
            _repoConvenios = repoConvenios;
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
        public override async Task<bool> DeleteAsync(int id)
        {
            var convenios = await _repoConvenios.ListarConveniosConEmpresaAsync();
            if (convenios.Any(c => c.IdEmpresa == id))
            {
                throw new Backend.Exceptions.ValidationException("No se puede eliminar la empresa porque tiene convenios asociados", "Empresa");
            }
            return await base.DeleteAsync(id);
        }
    }
}
