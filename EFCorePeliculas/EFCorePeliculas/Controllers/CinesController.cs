using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCorePeliculas.Controllers.DTOs;
using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/cines")]
    public class CinesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public CinesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
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
                Nombre = "Mi Cine",
                Ubicacion = ubicacionCine,
                CineOferta = new CineOferta()
                {
                    PorcentajeDescuento = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(7)
                },
                SalasDeCine = new HashSet<SalaDeCine>()
                {
                    new SalaDeCine()
                    {
                        TipoSalaDeCine = TipoSalaDeCine.DosDimensiones,
                        Precio = 200
                    },
                    new SalaDeCine()
                    {
                        TipoSalaDeCine = TipoSalaDeCine.TresDimensiones,
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
    }
}
