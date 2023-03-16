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
            return await context.Generos.ToListAsync();
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
    }
}
