using AutoMapper;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    public class ServicioConvenios : BaseService<Convenio, ConvenioDto, ConvenioCreateDto>
    {
        private readonly IRepositorioConvenios _repoConvenios;

        public ServicioConvenios(IRepositorioConvenios repository, IMapper mapper) : base(repository, mapper)
        {
            _repoConvenios = repository;
        }

        protected override int GetIdFromDto(ConvenioDto dto)
        {
            return dto.IdConvenio;
        }

        // Métodos específicos para convenios pueden agregarse aquí
    }
}
