using EFCorePeliculas.Entidades;

namespace EFCorePeliculas.Controllers.DTOs
{
    public class SalaDeCineCreacionDTO
    {
        public decimal Precio { get; set; }
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
    }
}
