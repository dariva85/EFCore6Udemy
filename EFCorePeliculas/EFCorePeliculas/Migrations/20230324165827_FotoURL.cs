using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class FotoURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoURL",
                table: "Actores",
                type: "varchar(500)",
                unicode: false,
                maxLength: 500,
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoURL",
                table: "Actores");
        }
    }
}
