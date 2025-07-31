using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo para Estudiante
            CreateMap<Estudiante, StudentDto>().ReverseMap();
        }
    }
}