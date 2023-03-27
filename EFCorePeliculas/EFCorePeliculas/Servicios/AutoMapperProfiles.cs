using AutoMapper;
using EFCorePeliculas.Controllers;
using EFCorePeliculas.Controllers.DTOs;
using EFCorePeliculas.Entidades;
using EFCorePeliculas.Migrations;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFCorePeliculas.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, ActorDTO>();
            CreateMap<Cine, CineDTO>()
                .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y))
                .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X));
            CreateMap<Genero, GeneroDTO>();
            //sin projectTo
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
                .ForMember(dto => dto.Actores, ent => ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

            //con ProjectTo
            //CreateMap<Pelicula, PeliculaDTO>()
            //    .ForMember(dto => dto.Generos, ent => ent.MapFrom(prop => prop.Generos.OrderByDescending(g => g.Nombre)))
            //    .ForMember(dto => dto.Cines, ent => ent.MapFrom(prop => prop.SalasDeCine.Select(s => s.Cine)))
            //    .ForMember(dto => dto.Actores, ent => ent.MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

            var GeometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            CreateMap<CineCreacionDTO, Cine>()
                .ForMember(ent=> ent.SalasDeCine, opciones => opciones.Ignore())
                .ForMember(dto => dto.Ubicacion, ent => ent.MapFrom(campo => GeometryFactory.CreatePoint(new Coordinate(campo.Longitud, campo.Latitud))));
            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>();
            CreateMap<CineOfertaCreacionDTO, CineOferta>();

            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(dto => dto.Generos, ent => ent.MapFrom(campo => campo.Generos.Select(id => new Genero() { Identificador = id })))
                .ForMember(dto => dto.SalasDeCine, ent => ent.MapFrom(campo => campo.SalasDeCine.Select(id => new SalaDeCine() { Id = id })));
                
            CreateMap<PeliculaActorCreacionDTO, PeliculaActor>();
            CreateMap<ActorCreacionDTO, Actor>();
        }
    }
}
