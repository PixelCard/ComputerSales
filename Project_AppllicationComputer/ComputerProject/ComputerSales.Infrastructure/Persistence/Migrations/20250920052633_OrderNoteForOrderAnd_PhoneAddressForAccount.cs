using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OrderNoteForOrderAnd_PhoneAddressForAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sdt",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "address",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "sdt",
                table: "Customer");
        }
    }
}
