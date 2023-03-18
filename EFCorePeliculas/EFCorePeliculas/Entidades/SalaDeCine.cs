namespace EFCorePeliculas.Entidades
{
    public class SalaDeCine
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public  int CineId { get; set; }
        public virtual Cine Cine { get; set; }
        public TipoSalaDeCine TipoSalaDeCine { get; set; }
        public virtual HashSet<Pelicula> Peliculas { get; set; }
    }
}
