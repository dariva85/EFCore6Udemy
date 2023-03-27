using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.Controllers.DTOs;
using EFCorePeliculas.Entidades;
using EFCorePeliculas.Entidades.SinLlave;
using EFCorePeliculas.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using System.Collections.ObjectModel;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/cines")]
    public class CinesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IActualizadorObservableCollection actualizadorObservableCollection;
        private readonly ApplicationDbContext context;

        public CinesController(ApplicationDbContext context, IMapper mapper, IActualizadorObservableCollection actualizadorObservableCollection)
        {
            this.context = context;
            this.mapper = mapper;
            this.actualizadorObservableCollection = actualizadorObservableCollection;
        }

        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("sinllave")]
        public async Task<IEnumerable<CineSinUbicacion>> GetSinLlave()
        {
            return await context.CinesSinUbicacion.ToListAsync();
        }

        [HttpGet("cercanos")]
        public async Task<ActionResult> Get(double latitud, double longitud)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var miUbicacion = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(longitud, latitud));
            var distanciaMaximaEnMetros = 2000;

            var cines = await context.Cines
                .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                .Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaximaEnMetros))
                .Select(c => new
                {
                    Nombre = c.Nombre,
                    Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
                })
                .ToListAsync();
            return Ok(cines);
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var ubicacionCine = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(-69.896979, 18.476276));

            var cine = new Cine()
            {
                Nombre = "Mi Cine con Monedas para probar table Split",
                Ubicacion = ubicacionCine,
                CineDetalle = new CineDetalle()
                {
                    Historia = "Historia...",
                    Mision = "Mision...",
                    Vision = "Vision..."
                },
                CineOferta = new CineOferta()
                {
                    PorcentajeDescuento = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(7)
                },
                SalasDeCine = new ObservableCollection<SalaDeCine>()
                {
                    new SalaDeCine()
                    {
                        TipoSalaDeCine = TipoSalaDeCine.DosDimensiones,
                        Moneda = Moneda.PesoDominicano,
                        Precio = 200

                    },
                    new SalaDeCine()
                    {
                        TipoSalaDeCine = TipoSalaDeCine.TresDimensiones,
                        Moneda = Moneda.DolarEstadounidense,
                        Precio = 350
                    }
                }
            };
            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("conDTO")]
        public async Task<ActionResult> Post(CineCreacionDTO cineCreacionDTO)
        {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CineCreacionDTO cineCreacionDTO, int id)
        {
            var cineDB = await context.Cines.AsTracking().Include(c => c.SalasDeCine).Include(c => c.CineOferta).FirstOrDefaultAsync(c => c.Id == id);

            if (cineDB is not null)
            {
                return NotFound();
            }

            cineDB = mapper.Map(cineCreacionDTO, cineDB);
            actualizadorObservableCollection.Actualizar(cineDB.SalasDeCine, cineCreacionDTO.SalasDeCine);


            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var cine = await context.Cines.Include(c => c.SalasDeCine).Include(c => c.CineOferta).FirstOrDefaultAsync(X => X.Id == id);

            if (cine is null)
            {
                return NotFound();
            }
            context.RemoveRange(cine.SalasDeCine);
            context.Remove(cine);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            //var cine = await context.Cines.Include(c=>c.CineDetalle).FirstOrDefaultAsync(c=>c.Id == id);

            var cine = await context.Cines
                .FromSqlInterpolated($"Select * FROM Cines WHERE Id = {id}")
                .Include(c => c.CineDetalle)
                .FirstOrDefaultAsync();

            if(cine is null)
            {
                return NotFound();
            }

            cine.Ubicacion = null;

            return Ok(cine);
        }
    }
}
