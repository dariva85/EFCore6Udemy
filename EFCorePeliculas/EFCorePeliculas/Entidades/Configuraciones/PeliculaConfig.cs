using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class PeliculaConfig : IEntityTypeConfiguration<Pelicula>
    {
        public void Configure(EntityTypeBuilder<Pelicula> builder)
        {

            builder.Property(prop => prop.Titulo)
                .HasMaxLength(250)
                .IsRequired();
            builder.Property(prop => prop.FechaEstreno)
                .HasColumnType("date");
            builder.Property(prop => prop.PosterURL)
                .HasMaxLength(500)
                .IsUnicode(false);

            //builder.HasMany(p => p.Generos)
            //    .WithMany(g => g.Peliculas)
            //    //Esto es por si quisieramos crear nuestra propia tabla intermedia con nuestros nombres
            //    .UsingEntity(j => j.ToTable("PeliculasGeneros")
            //            .HasData(new { PeliculasId = 1, GenerosIdentificador = 7 })
            //            );
        }
    }
}
