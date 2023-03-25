using Microsoft.EntityFrameworkCore;

namespace EFCorePeliculas.Entidades.Seeding
{
    public static class SeedingPersonaMensaje
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var felipe = new Persona() { Id = 1, Nombre = "Felipe" };
            var Claudia = new Persona() { Id = 2, Nombre = "Claudia" };

            var mensaje1 = new Mensaje() { Id = 1, Contenido = "Hola, Claudia", EmisorId = felipe.Id, ReceptorId = Claudia.Id };
            var mensaje2 = new Mensaje() { Id = 2, Contenido = "Hola, Felipe, ¿Cómo te va?", EmisorId = Claudia.Id, ReceptorId = felipe.Id };
            var mensaje3 = new Mensaje() { Id = 3, Contenido = "Todo bien, ¿y tu?", EmisorId = felipe.Id, ReceptorId = Claudia.Id };
            var mensaje4 = new Mensaje() { Id = 4, Contenido = "Muy bien :)", EmisorId = Claudia.Id, ReceptorId = felipe.Id };

            modelBuilder.Entity<Persona>().HasData(felipe, Claudia);
            modelBuilder.Entity<Mensaje>().HasData(mensaje1, mensaje2, mensaje3, mensaje4);
        }
    }
}
