﻿using EFCorePeliculas.Entidades.Configuraciones.Conversiones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class SalaDeCineConfig : IEntityTypeConfiguration<SalaDeCine>
    {
        public void Configure(EntityTypeBuilder<SalaDeCine> builder)
        {
            builder.Property(prop => prop.Precio)
                .HasPrecision(precision: 9, scale: 2);
            builder.Property(prop => prop.TipoSalaDeCine)
                //.HasDefaultValueSql("GETDATE()");
                .HasDefaultValue(TipoSalaDeCine.DosDimensiones)
                .HasConversion<string>();

            builder.Property(sdc => sdc.Moneda).HasConversion<MonedaSimboloConverter>();
        }
    }
}
