using AutoMapper;
using Backend.DTOs.AuditoriaDtos;
using Backend.DTOs.ConvenioDtos;
using Backend.DTOs.PasantiaDtos;
using Backend.DTOs.StudentDtos;
using Backend.DTOs.EmpresaDtos;
using Backend.DTOs.PagosDtos;
using Backend.Models;
namespace Backend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo para Convenio
            CreateMap<Convenio, ConvenioUpdateDto>().ReverseMap();
            CreateMap<Convenio, ConvenioDto>().ReverseMap(); // Para operaciones GET/CREATE
            CreateMap<ConvenioCreateDto, Convenio>();

            // Mapeo para Estudiante
            CreateMap<Estudiante, StudentUpdateDto>().ReverseMap();
            CreateMap<Estudiante, StudentDto>().ReverseMap(); // Para operaciones GET/CREATE
            CreateMap<StudentCreateDto, Estudiante>();

            // Mapeo para Empresa
            CreateMap<Empresa, EmpresaUpdateDto>();
            CreateMap<EmpresaUpdateDto, Empresa>()
                .ForMember(dest => dest.Eliminado, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaEliminacion, opt => opt.MapFrom(src => (DateTime?)null));
            
            CreateMap<Empresa, EmpresaDto>()
                .ReverseMap()
                .ForMember(dest => dest.Eliminado, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaEliminacion, opt => opt.MapFrom(src => (DateTime?)null)); // Para operaciones GET/CREATE
            CreateMap<EmpresaCreateDto, Empresa>();

            // Mapeo para Pasantía
            CreateMap<Pasantia, PasantiaUpdateDto>()
                .ForMember(dest => dest.HorasSemanales, opt => opt.MapFrom(src => src.HorasSemanales))
                .ForMember(dest => dest.DniEstudiante, opt => opt.MapFrom(src => src.IdEstudianteNavigation != null ? src.IdEstudianteNavigation.Documento : null));
            CreateMap<PasantiaUpdateDto, Pasantia>()
                .ForMember(dest => dest.IdEstudiante, opt => opt.Ignore()) // Ignorar porque usamos DNI y lo asignamos manualmente
                .ForMember(dest => dest.IdEstudianteNavigation, opt => opt.Ignore());
            CreateMap<PasantiaUpdateDto, PasantiaDto>();
            
            CreateMap<Pasantia, PasantiaDto>()
                .ForMember(dest => dest.Tramite, opt => opt.MapFrom(src => $"TRA-FACET-{src.IdPasantia:D3}"))
                .ForMember(dest => dest.HorasSemanales, opt => opt.MapFrom(src => src.HorasSemanales))
                .ForMember(dest => dest.AreaTrabajo, opt => opt.MapFrom(src => src.IdEstudianteNavigation != null ? src.IdEstudianteNavigation.AreaTrabajo : null))
                .ReverseMap()
                .ForMember(dest => dest.IdEstudianteNavigation, opt => opt.Ignore()); // Compatibilidad
            CreateMap<PasantiaCreateDto, Pasantia>()
                .ForMember(dest => dest.IdEstudiante, opt => opt.Ignore()) // Ignorar porque usamos DNI y lo asignamos manualmente
                .ForMember(dest => dest.IdConvenio, opt => opt.MapFrom(src => src.IdConvenio))
                .ForMember(dest => dest.AsignacionMensual, opt => opt.MapFrom(src => src.AsignacionMensual))
                .ForMember(dest => dest.ObraSocial, opt => opt.MapFrom(src => src.ObraSocial))
                .ForMember(dest => dest.Art, opt => opt.MapFrom(src => src.Art))
                .ForMember(dest => dest.TutorEmpresa, opt => opt.MapFrom(src => src.TutorEmpresa))
                .ForMember(dest => dest.DniTutorFacultad, opt => opt.MapFrom(src => src.DniTutorFacultad))
                .ForMember(dest => dest.TutorFacultad, opt => opt.MapFrom(src => src.TutorFacultad))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.FechaFin))
                .ForMember(dest => dest.TipoAcuerdo, opt => opt.MapFrom(src => src.TipoAcuerdo))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
                .ForMember(dest => dest.FrecuenciaPago, opt => opt.MapFrom(src => src.FrecuenciaPago))
                .ForMember(dest => dest.Sudocu, opt => opt.MapFrom(src => src.Sudocu))
                .ForMember(dest => dest.HorasSemanales, opt => opt.MapFrom(src => src.HorasSemanales));

            // Mapeo para Auditoría
            CreateMap<Auditoria, AuditoriaUpdateDto>();
            CreateMap<AuditoriaUpdateDto, Auditoria>()
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore());
            
            CreateMap<Auditoria, AuditoriaDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.IdUsuarioNavigation != null ? src.IdUsuarioNavigation.NombreUsuario : null))
                .ReverseMap()
                .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore()); // Para operaciones GET/CREATE
            CreateMap<AuditoriaBuscarDto, Auditoria>();
            
            // Mapeo para Pago
            CreateMap<Pago, PagosUpdateDto>().ReverseMap();
            CreateMap<Pago, PagosDto>().ReverseMap(); // Para operaciones GET/CREATE
            CreateMap<CreatePagosDto, Pago>();
            CreateMap<PagosUpdateDto, PagosDto>();
        }
    }
}