using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class DatosFacturaEjemplo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalasDeCine_Cines_ElCine",
                table: "SalasDeCine");

            migrationBuilder.InsertData(
                table: "Facturas",
                columns: new[] { "Id", "FechaCreacion" },
                values: new object[,]
                {
                    { 2, new DateTime(2022, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2022, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2022, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2022, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "FacturaDetalles",
                columns: new[] { "Id", "FacturaId", "Precio", "Producto" },
                values: new object[,]
                {
                    { 3, 2, 350.99m, null },
                    { 4, 2, 10m, null },
                    { 5, 2, 45.50m, null },
                    { 6, 3, 17.99m, null },
                    { 7, 3, 14m, null },
                    { 8, 3, 45m, null },
                    { 9, 3, 100m, null },
                    { 10, 4, 371m, null },
                    { 11, 4, 114.99m, null },
                    { 12, 4, 425m, null },
                    { 13, 4, 1000m, null },
                    { 14, 4, 5m, null },
                    { 15, 4, 2.99m, null },
                    { 16, 5, 50m, null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SalasDeCine_Cines_ElCine",
                table: "SalasDeCine",
                column: "ElCine",
                principalTable: "Cines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalasDeCine_Cines_ElCine",
                table: "SalasDeCine");

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "FacturaDetalles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Facturas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Facturas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Facturas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Facturas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.AddForeignKey(
                name: "FK_SalasDeCine_Cines_ElCine",
                table: "SalasDeCine",
                column: "ElCine",
                principalTable: "Cines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
