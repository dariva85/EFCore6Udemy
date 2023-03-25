using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.Controllers.DTOs;
using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.SinLlave;
using EFCorePeliculas.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Runtime.CompilerServices;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PeliculasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("PeliculasConConteos")]
        public async Task<IEnumerable<PeliculaConConteos>> GetPeliculasConteos()
        {
            return await context.Set<PeliculaConConteos>().ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas
                .Include(p => p.Generos.OrderByDescending(g => g.Nombre))
                .Include(p => p.SalasDeCine)
                    .ThenInclude(s => s.Cine)
                .Include(p => p.PeliculasActores.Where(pa => pa.Actor.FechaNacimiento.Value.Year >= 1980))
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(c => c.Id).ToList();

            return Ok(peliculaDTO);
        }

        [HttpGet("conprojectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            pelicula.Cines = pelicula.Cines.DistinctBy(c => c.Id).ToList();

            return Ok(pelicula);
        }

        [HttpGet("cargadoselectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await context.Peliculas.Select(p =>
            new
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre).ToList(),
                CantidadActores = p.PeliculasActores.Count(),
                Cines = p.SalasDeCine.Select(s => s.ElCine).Distinct().Count()
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }

        [HttpGet("cargadoexplicito/{id:int}")]
        public async Task<ActionResult> GetExplicito(int id)
        {
            var pelicula = await context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);

            await context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();

            var cantidadGeneros = await context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return Ok(peliculaDTO);
        }

        [HttpGet("lazyloading/{id:int}")]
        public async Task<ActionResult<List<PeliculaDTO>>> GetLazyLoading(int id)
        {
            var peliculas = await context.Peliculas.AsTracking().ToListAsync();

            foreach (var pelicula in peliculas)
            {
                //cargar los generos de la pelicula
                //problema n + 1
                pelicula.Generos.ToList();
            }

            var peliculaDTOs = mapper.Map<PeliculaDTO>(peliculas);

            return Ok(peliculaDTOs);
        }

        [HttpGet("agrupadasporestreno")]
        public async Task<ActionResult> GetAgrupadasPorCartelera()
        {
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.EnCartelera)
                .Select(p => new
                {
                    EnCartelera = p.Key,
                    Conteo = p.Count(),
                    Peliculas = p.ToList()
                }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }

        [HttpGet("agrupadasporcantidaddegeneros")]
        public async Task<ActionResult> GetAgrupadasPorCantidadDeGeneros()
        {
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.Generos.Count())
                .Select(p => new
                {
                    Conteo = p.Key,
                    Titulos = p.Select(x => x.Titulo),
                    Generos = p.Select(p => p.Generos).SelectMany(gen => gen).Select(gen => gen.Nombre).Distinct()
                }).ToListAsync();

            return Ok(peliculasAgrupadas);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar(
            [FromQuery] PeliculasFiltroDTO peliculasFiltroDTO)
        {
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo))
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
            }

            if (peliculasFiltroDTO.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
            }

            if (peliculasFiltroDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
            }

            if (peliculasFiltroDTO.GeneroId != 0)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.Generos.Select(g => g.Identificador)
                                        .Contains(peliculasFiltroDTO.GeneroId));
            }


            var peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            pelicula.Generos.ForEach(g => { context.Entry(g).State = EntityState.Unchanged; });
            pelicula.SalasDeCine.ForEach(s => { context.Entry(s).State = EntityState.Unchanged; });

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
