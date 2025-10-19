using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CapNhat_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview");

            migrationBuilder.DropColumn(
                name: "BlockType",
                table: "ProductOverview");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "ProductOverview");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductOverview");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductOverview");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview");

            migrationBuilder.AddColumn<int>(
                name: "BlockType",
                table: "ProductOverview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "ProductOverview",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductOverview",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductOverview",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOverview_ProductId",
                table: "ProductOverview",
                column: "ProductId");
        }
    }
}
