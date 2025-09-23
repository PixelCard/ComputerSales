using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.PaymentID);
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentID", "Code", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "COD", true, "Cash on Delivery" },
                    { 2, "ZALOPAY", true, "ZaloPay" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_PaymentID",
                table: "Order",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_Code",
                table: "PaymentMethod",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PaymentMethod_PaymentID",
                table: "Order",
                column: "PaymentID",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PaymentMethod_PaymentID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropIndex(
                name: "IX_Order_PaymentID",
                table: "Order");
        }
    }
}
