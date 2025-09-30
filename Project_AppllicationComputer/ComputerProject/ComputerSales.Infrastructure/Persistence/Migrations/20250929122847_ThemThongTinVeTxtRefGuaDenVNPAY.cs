using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ThemThongTinVeTxtRefGuaDenVNPAY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeqId",
                table: "VNPAYPaymentSessions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "TxnRef",
                table: "VNPAYPaymentSessions",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions",
                column: "TxnRef",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_VNPAYSession_TxnRef",
                table: "VNPAYPaymentSessions");

            migrationBuilder.DropColumn(
                name: "SeqId",
                table: "VNPAYPaymentSessions");

            migrationBuilder.DropColumn(
                name: "TxnRef",
                table: "VNPAYPaymentSessions");
        }
    }
}
