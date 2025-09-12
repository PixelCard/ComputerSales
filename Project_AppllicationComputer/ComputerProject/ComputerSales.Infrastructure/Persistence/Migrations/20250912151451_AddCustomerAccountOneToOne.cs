using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerAccountOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDAccount",
                table: "Customer",
                type: "int",
                nullable: true); // tạm thời cho phép null để cập nhật dữ liệu

            // (Tùy dữ liệu thực tế) Điền IDAccount cho các Customer hiện có
            // Ví dụ: dựa trên một tiêu chí khớp, bạn tự thay câu UPDATE bên dưới
            // migrationBuilder.Sql("UPDATE c SET IDAccount = a.IDAccount FROM Customer c JOIN Account a ON a.Email = c.Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IDAccount",
                table: "Customer",
                column: "IDAccount",
                unique: true,
                filter: "[IDAccount] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Account_IDAccount",
                table: "Customer",
                column: "IDAccount",
                principalTable: "Account",
                principalColumn: "IDAccount",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_Customer_Account_IDAccount", "Customer");
            migrationBuilder.DropIndex("IX_Customer_IDAccount", "Customer");
            migrationBuilder.DropColumn("IDAccount", "Customer");
        }
    }
}
