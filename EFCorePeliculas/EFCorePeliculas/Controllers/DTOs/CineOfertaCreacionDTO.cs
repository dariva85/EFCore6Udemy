using System.ComponentModel.DataAnnotations;

namespace EFCorePeliculas.Controllers.DTOs
{
    public class CineOfertaCreacionDTO
    {
        [Range(0, 100)]
        public double PorcentajeDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
