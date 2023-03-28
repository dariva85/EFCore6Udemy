using EFCorePeliculas.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EFCorePeliculas.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public FacturasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            using var transaccion = await context.Database.BeginTransactionAsync();
            try
            {
                var factura = new Factura();

                context.Add(factura);
                await context.SaveChangesAsync();

                var facturaDetalle = new List<FacturaDetalle>()
                {
                    new FacturaDetalle()
                    {
                        Producto = "Producto A",
                        Precio = 123,
                        FacturaId = factura.Id
                    },
                    new FacturaDetalle()
                    {
                        Producto = "Producto B",
                        Precio = 456,
                        FacturaId = factura.Id
                    }
                };
                context.AddRange(facturaDetalle);
                await context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return Ok("todo bien gracias");
            }
            catch (Exception ex) { return BadRequest("Hubo un error)"); }
        }
        [HttpGet("FuncionesEscalares")]
        public async Task<ActionResult> GetFuncionesEscalares()
        {
            var facturas = await context.Facturas.Select(f => new
            {
                id = f.Id,
                Total = context.FacturaDetalleSuma2(f.Id),
                Promedio = context.FacturaDetallePromedio(f.Id)
            }).OrderByDescending(x => context.FacturaDetalleSuma2(x.id)).ToListAsync();

            return Ok(facturas);
        }
    }
}
