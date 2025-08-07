using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo para Convenio
            CreateMap<Convenio, ConvenioDto>().ReverseMap();
            CreateMap<ConvenioCreateDto, Convenio>();

            // Mapeo para Estudiante
            CreateMap<Estudiante, StudentDto>().ReverseMap();
            CreateMap<StudentCreateDto, Estudiante>();
            CreateMap<StudentUpdateDto, Estudiante>();

            // Mapeo para Empresa
            CreateMap<Empresa, EmpresaDto>().ReverseMap()
                .ForMember(dest => dest.Eliminado, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaEliminacion, opt => opt.MapFrom(src => (DateTime?)null));

            // Mapeo para Pasantía
            CreateMap<Pasantia, PasantiaDto>().ReverseMap();
            CreateMap<PasantiaCreateDto, Pasantia>();
        }
    }
}