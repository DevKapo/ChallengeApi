using AutoMapper;
using ChallengeApi.DTOs;
using ChallengeApi.Entities;

namespace ChallengeApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entrada: PeliculaCrearDto -> Pelicula
            CreateMap<PeliculaCrearDto, Pelicula>()
                .ForMember(dest => dest.ProductoraId, opt => opt.MapFrom(src => src.ProductoraNombre))
                .ForMember(dest => dest.Portada, opt => opt.MapFrom(src => src.Portada))
                .ForMember(dest => dest.Generos, opt => opt.Ignore())
                .ForMember(dest => dest.Actores, opt => opt.Ignore());

            // Salida: Pelicula -> PeliculaDto
            CreateMap<Pelicula, PeliculaDto>()
                .ForMember(dest => dest.ProductoraNombre, opt => opt.MapFrom(src => src.Productora.Nombre))
                .ForMember(dest => dest.Generos, opt => opt.MapFrom(src => src.Generos.Select(g => g.Nombre)))
                .ForMember(dest => dest.Actores, opt => opt.MapFrom(src => src.Actores.Select(a => a.Nombre)))
                .ForMember(dest => dest.Portada, opt => opt.MapFrom(src => src.Portada));

            // Mapeo entidad Portada -> DTO
            CreateMap<Portada, PortadaDto>()
                .ForMember(dest => dest.PeliculaNombre, opt => opt.MapFrom(src => src.Pelicula.Nombre));


            // Entrada: ProductoraCrearDto -> Productora
            CreateMap<ProductoraCrearDto, Productora>();

            // Salida: Productora -> ProductoraDto 
            CreateMap<Productora, ProductoraDto>();

            // Salida: Genero -> GeneroDto
            CreateMap<Genero, GeneroDto>();
        }
    }
}
