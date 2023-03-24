using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class PeliculasConConteos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[PeliculasConConteos]
                                    AS
                                    SELECT Id, Titulo,
                                    (SELECT count(*)
	                                    FROM GeneroPelicula
	                                    WHERE PeliculasId = Peliculas.Id) as CantidadGeneros,
                                    (SELECT count(distinct(CineId))
	                                    FROM SalasDeCine
	                                    INNER JOIN PeliculaSalaDeCine
	                                    ON SalasDeCine.Id = PeliculaSalaDeCine.SalasDeCineId
	                                    WHERE PeliculaSalaDeCine.PeliculasId = Peliculas.Id) AS CantidadCines,
                                    (SELECT count(*)
	                                    FROM PeliculasActores
	                                    WHERE PeliculasActores.PeliculaId = Peliculas.Id) AS CantidadActores
                                    FROM Peliculas
                                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[PeliculasConConteos]");
        }
    }
}
