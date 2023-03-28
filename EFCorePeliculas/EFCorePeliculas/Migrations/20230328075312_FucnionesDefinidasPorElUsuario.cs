using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    /// <inheritdoc />
    public partial class FucnionesDefinidasPorElUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE FUNCTION FacturaDetalleSuma
(
@FacturaId INT
)
RETURNS int
AS
BEGIN -- Declare the return variable here
DECLARE @suma INT;

--Add the T-SQL statements to compute the return value here
SELECT @suma = SUM(Precio)
FROM FacturaDetalles
WHERE FacturaId = @FacturaId

--Return the result of the funcion
RETURN @suma

END
");

            migrationBuilder.Sql(@"
CREATE FUNCTION FacturaDetallePromedio
(
@FacturaId INT
)
RETURNS decimal(18,2)
AS
BEGIN
-- Declare the return variable here
DECLARE @promedio decimal(18,2)

--Add the T-SQL statements to compute the return value here
SELECT @promedio = AVG(Precio)
FROM FacturaDetalles
WHERE FacturaId = @FacturaId

--Return the result of the funcion
RETURN @promedio

END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION [dbo].[FacturaDetalleSuma]");
            migrationBuilder.Sql("DROP FUNCTION [dbo].[FacturaDetallePromedio]");
        }
    }
}
