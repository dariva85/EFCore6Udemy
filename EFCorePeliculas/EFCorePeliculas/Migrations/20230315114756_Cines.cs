using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class Cines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cine",
                table: "Cine");

            migrationBuilder.RenameTable(
                name: "Cine",
                newName: "Cines");

            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Cines",
                type: "decimal(9,2)",
                precision: 9,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cines",
                table: "Cines",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cines",
                table: "Cines");

            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Cines");

            migrationBuilder.RenameTable(
                name: "Cines",
                newName: "Cine");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cine",
                table: "Cine",
                column: "Id");
        }
    }
}
