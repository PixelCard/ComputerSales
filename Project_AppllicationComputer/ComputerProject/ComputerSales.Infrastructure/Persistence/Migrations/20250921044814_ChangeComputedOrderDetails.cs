using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeComputedOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // OrderDetails.TotalPrice = (UnitPrice - Discount) * Quantity
            migrationBuilder.Sql(@"
            IF COL_LENGTH('dbo.[OrderDetails]', 'TotalPrice') IS NOT NULL
            ALTER TABLE dbo.[OrderDetails] DROP COLUMN [TotalPrice];
            ALTER TABLE dbo.[OrderDetails]
            ADD [TotalPrice] AS (([UnitPrice]-[Discount]) * [Quantity]) PERSISTED;
            ");

            // Order.GrandTotal = Subtotal - DiscountTotal + ShippingFee
            migrationBuilder.Sql(@"
            IF COL_LENGTH('dbo.[Order]', 'GrandTotal') IS NOT NULL
            ALTER TABLE dbo.[Order] DROP COLUMN [GrandTotal];
            ALTER TABLE dbo.[Order]
            ADD [GrandTotal] AS ([Subtotal]-[DiscountTotal]+[ShippingFee]) PERSISTED;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF COL_LENGTH('dbo.[OrderDetails]', 'TotalPrice') IS NOT NULL
                                ALTER TABLE dbo.[OrderDetails] DROP COLUMN [TotalPrice];");
            migrationBuilder.Sql(@"IF COL_LENGTH('dbo.[Order]', 'GrandTotal') IS NOT NULL
                                ALTER TABLE dbo.[Order] DROP COLUMN [GrandTotal];");
        }
    }
}
