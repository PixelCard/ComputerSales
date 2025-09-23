using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusAndConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Orders_OrderStatus_ValidRange",
                table: "Order",
                sql: "[OrderStatus] BETWEEN 0 AND 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Orders_OrderStatus_ValidRange",
                table: "Order");
        }
    }
}
