﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCorePeliculas.Entidades.Configuraciones
{
    public class ActorConfig : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.Property(prop => prop.Nombre)
                .HasMaxLength(150)
                .IsRequired();
            builder.Property(prop => prop.FechaNacimiento)
                .HasColumnType("date");

            builder.Property(p => p.Nombre)
                .HasField("_nombre");

            //builder.Ignore(a => a.Edad);
        }
    }
}
