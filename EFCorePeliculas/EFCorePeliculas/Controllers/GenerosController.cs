using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            return await context.Generos.OrderBy(g => g.Nombre).ToListAsync();
        }

        [HttpGet("primer")]
        public async Task<ActionResult<Genero>> Primer()
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Nombre.StartsWith("z"));

            if (genero == null)
            {
                return NotFound();
            }
            return genero;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Primer(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }
            return genero;
        }

        [HttpGet("filtrar")]
        public async Task<IEnumerable<Genero>> Filtrar()
        {
            return await context.Generos.Where(g =>
            g.Nombre.StartsWith("C") || g.Nombre.StartsWith("A")
            ).ToListAsync();

        }

        [HttpGet("filtrarNombre")]
        public async Task<IEnumerable<Genero>> Filtrar(string nombre)
        {
            return await context.Generos
                .Where(g => g.Nombre.Contains(nombre))
                .OrderBy(g => g.Nombre)
                //.OrderByDescending(g => g.Nombre)
                .ToListAsync();
        }

        [HttpGet("paginacion")]
        public async Task<IEnumerable<Genero>> GetPaginacion(int pagina = 1)
        {
            var cantidadRegistorPorPagina = 2;
            var generos = await context.Generos
                .Skip((pagina -1) * cantidadRegistorPorPagina)
                .Take(cantidadRegistorPorPagina).ToListAsync();
            return generos;
        }
    }
}
