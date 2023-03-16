using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class CineOfertaConfig : IEntityTypeConfiguration<CineOferta>
    {
        public void Configure(EntityTypeBuilder<CineOferta> builder)
        {
            builder.Property(prop => prop.PorcentajeDescuento)
        .HasPrecision(precision: 5, scale: 2);
            builder.Property(prop => prop.FechaFin)
                    .HasColumnType("date");
            builder.Property(prop => prop.FechaInicio)
                    .HasColumnType("date");
        }
    }
}
