using EFCorePeliculas.Entidades;

namespace EFCorePeliculas.Controllers.DTOs
{
    public class CineCreacionDTO
    {
        public string Nombre { get; set; }
        //[Precision(precision:9, scale:2)]
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public CineOfertaCreacionDTO CineOferta { get; set; }
        public SalaDeCineCreacionDTO[] SalasDeCine { get; set; }
    }
}
