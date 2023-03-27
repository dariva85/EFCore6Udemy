using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class GeneroEntidadAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                table: "Generos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                table: "Generos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Generos",
                keyColumn: "Identificador",
                keyValue: 1,
                columns: new[] { "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Generos",
                keyColumn: "Identificador",
                keyValue: 2,
                columns: new[] { "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Generos",
                keyColumn: "Identificador",
                keyValue: 3,
                columns: new[] { "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Generos",
                keyColumn: "Identificador",
                keyValue: 4,
                columns: new[] { "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Generos",
                keyColumn: "Identificador",
                keyValue: 5,
                columns: new[] { "UsuarioCreacion", "UsuarioModificacion" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                table: "Generos");
        }
    }
}
