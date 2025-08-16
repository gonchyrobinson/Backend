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

            // Mapeo para Empresa
            CreateMap<Empresa, EmpresaDto>().ReverseMap()
                .ForMember(dest => dest.Eliminado, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaEliminacion, opt => opt.MapFrom(src => (DateTime?)null));
            CreateMap<EmpresaCreateDto, Empresa>();

            // Mapeo para Pasantía
            CreateMap<Pasantia, PasantiaDto>().ReverseMap();
            CreateMap<PasantiaCreateDto, Pasantia>()
                .ForMember(dest => dest.IdEstudiante, opt => opt.MapFrom(src => src.IdEstudiante))
                .ForMember(dest => dest.IdConvenio, opt => opt.MapFrom(src => src.IdConvenio))
                .ForMember(dest => dest.AsignacionMensual, opt => opt.MapFrom(src => src.AsignacionMensual))
                .ForMember(dest => dest.ObraSocial, opt => opt.MapFrom(src => src.ObraSocial))
                .ForMember(dest => dest.Art, opt => opt.MapFrom(src => src.Art))
                .ForMember(dest => dest.TutorEmpresa, opt => opt.MapFrom(src => src.TutorEmpresa))
                .ForMember(dest => dest.DniTutorFacultad, opt => opt.MapFrom(src => src.dniTutorFacultad))
                .ForMember(dest => dest.TutorFacultad, opt => opt.MapFrom(src => src.TutorFacultad))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.TipoAcuerdo, opt => opt.MapFrom(src => src.TipoAcuerdo))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.FrecuenciaPago, opt => opt.MapFrom(src => src.FrecuenciaPago))
                .ForMember(dest => dest.MontoPago, opt => opt.MapFrom(src => src.MontoPago))
                .ForMember(dest => dest.Sudocu, opt => opt.MapFrom(src => src.Sudocu));

            // Mapeo para Auditoría
            CreateMap<Auditoria, AuditoriaDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.IdUsuarioNavigation != null ? src.IdUsuarioNavigation.NombreUsuario : null))
                .ReverseMap()
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore());
            CreateMap<AuditoriaBuscarDto, Auditoria>();
            // Mapeo para Pago
            CreateMap<Pago, PagosDto>().ReverseMap();
            CreateMap<CreatePagosDto, Pago>();
        }
    }
}