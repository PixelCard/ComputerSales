using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CapNhatTruongDuLieuOptionalValueProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "VariantName",
                table: "ProductVariant",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OptionalValue",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "VariantName",
                table: "ProductVariant");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OptionalValue");
        }
    }
}
