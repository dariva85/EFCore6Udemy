using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class CineConfig : IEntityTypeConfiguration<Cine>
    {
        public void Configure(EntityTypeBuilder<Cine> builder)
        {
            builder.Property(prop => prop.Nombre)
                .HasMaxLength(150)
                .IsRequired();

            //En nuestro caso no hace falta ya que usamos los nombres según convención.
            builder.HasOne(c => c.CineOferta)
                .WithOne()
                .HasForeignKey<CineOferta>(co => co.CineId);

            builder.HasMany(c => c.SalasDeCine)
                .WithOne(sc => sc.Cine)
                .HasForeignKey(sci => sci.ElCine);
        }
    }
}
