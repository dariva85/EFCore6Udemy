﻿using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> Get()
        {
            context.Logs.Add(new Log { 
                Id = Guid.NewGuid(),
                Mensaje = "Ejecuntando el método GenerosController.Get" });
            await context.SaveChangesAsync();
            return await context.Generos.OrderByDescending(g => EF.Property<DateTime>(g, "FechaCreacion")).ToListAsync();
            //return await context.Generos.OrderBy(g => g.Nombre).ToListAsync();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Primer(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero == null)
            {
                return NotFound();
            }

            var fechaCreacion = context.Entry(genero).Property<DateTime>("FechaCreacion").CurrentValue;

            return Ok(new
            {
                Id = genero.Identificador,
                Nombre = genero.Nombre,
                FechaCreacion = fechaCreacion
            });
        }

        [HttpPost]
        public async Task<ActionResult> Post(Genero genero)
        {
            var existeGenero = await context.Generos.AnyAsync(g => g.Nombre == genero.Nombre);
            
            if(existeGenero)
            {
                return BadRequest("Ya existe un genero con el nombre: " + genero.Nombre);
            }
            
            context.Add(genero);
            await context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost("varios")]
        public async Task<ActionResult> Post(Genero[] generos)
        {
            context.AddRange(generos);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Agregar2(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.Nombre += " 2";
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genero = context.Generos.FirstOrDefault(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            context.Remove(genero);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = true;
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("restaurar/{id:int}")]
        public async Task<ActionResult> Restaurar(int id)
        {
            var genero = await context.Generos.AsTracking().IgnoreQueryFilters().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = false;
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
