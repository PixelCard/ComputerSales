using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTxnRefFilteredUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions");

            migrationBuilder.AlterColumn<string>(
                name: "TxnRef",
                table: "VNPAYPaymentSessions",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions",
                column: "TxnRef",
                unique: true,
                filter: "[TxnRef] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions");

            migrationBuilder.AlterColumn<string>(
                name: "TxnRef",
                table: "VNPAYPaymentSessions",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldUnicode: false,
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions",
                column: "TxnRef",
                unique: true);
        }
    }
}
