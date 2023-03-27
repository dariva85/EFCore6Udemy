using EFCorePeliculas.Entidades;

namespace EFCorePeliculas.Controllers.DTOs
{
    public class SalaDeCineCreacionDTO: IId
    {
        public decimal Precio { get; set; }
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
        public int Id { get; set; }
    }
}
