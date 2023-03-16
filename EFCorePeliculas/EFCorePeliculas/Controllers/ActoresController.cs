using EFCorePeliculas.Controllers.DTOs;
using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ActoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await context.Actores
                .Select(a=> new ActorDTO{id = a.Id, Nombre = a.Nombre})
                .ToListAsync();
        }
    }
}
