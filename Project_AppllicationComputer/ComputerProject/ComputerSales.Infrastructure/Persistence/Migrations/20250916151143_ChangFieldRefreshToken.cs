using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    public partial class ChangFieldRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Nếu trước đây cột datetime tên là "Revoked", đổi thành "RevokedAt"
            migrationBuilder.RenameColumn(
                name: "Revoked",
                table: "RefreshTokens",
                newName: "RevokedAt");

            // Nếu lỡ có cột bool "Revoked" do migration trước tạo nhầm, drop nó an toàn
            migrationBuilder.Sql(@"
                IF COL_LENGTH('RefreshTokens','Revoked') IS NOT NULL
                    ALTER TABLE [RefreshTokens] DROP COLUMN [Revoked];
            ");

            // Gỡ các cột audit không dùng nữa (nếu tồn tại)
            migrationBuilder.Sql(@"IF COL_LENGTH('RefreshTokens','Created') IS NOT NULL        ALTER TABLE [RefreshTokens] DROP COLUMN [Created];");
            migrationBuilder.Sql(@"IF COL_LENGTH('RefreshTokens','CreatedByIp') IS NOT NULL    ALTER TABLE [RefreshTokens] DROP COLUMN [CreatedByIp];");
            migrationBuilder.Sql(@"IF COL_LENGTH('RefreshTokens','ReplacedByToken') IS NOT NULL ALTER TABLE [RefreshTokens] DROP COLUMN [ReplacedByToken];");
            migrationBuilder.Sql(@"IF COL_LENGTH('RefreshTokens','RevokedByIp') IS NOT NULL    ALTER TABLE [RefreshTokens] DROP COLUMN [RevokedByIp];");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Khôi phục các cột audit nếu cần
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByIp",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplacedByToken",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevokedByIp",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: true);

            // Đổi tên "RevokedAt" về "Revoked" (kiểu datetime2) nếu rollback
            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "RefreshTokens",
                newName: "Revoked");
        }
    }
}
