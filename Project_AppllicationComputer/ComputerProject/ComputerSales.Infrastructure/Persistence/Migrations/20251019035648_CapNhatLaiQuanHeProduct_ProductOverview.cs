using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CapNhatLaiQuanHeProduct_ProductOverview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview",
                column: "ProductId",
                unique: true);
        }
    }
}
