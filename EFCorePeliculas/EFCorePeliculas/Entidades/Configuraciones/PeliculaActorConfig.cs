using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
    {
        public void Configure(EntityTypeBuilder<PeliculaActor> builder)
        {
            builder.HasKey(prop => new { prop.PeliculaId, prop.ActorId });
            builder.Property(prop => prop.Personaje)
                .HasMaxLength(150);

            builder.HasOne(pa => pa.Pelicula)
                .WithMany(p => p.PeliculasActores)
                .HasForeignKey(pa => pa.PeliculaId);

            builder.HasOne(pa => pa.Actor)
                .WithMany(a => a.PeliculasActores)
                .HasForeignKey(pa => pa.ActorId);
        }
    }
}
