using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmailVerifyTable_AccountLinkToEmailVerifyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutUntil",
                table: "Account",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifyKeyExpiresAt",
                table: "Account",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "VerifySendCountDate",
                table: "Account",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VerifySendCountToday",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmailVerifyKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    KeyHash = table.Column<string>(type: "nvarchar(88)", maxLength: 88, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerifyKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerifyKeys_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "IDAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerifyKeys_AccountId_KeyHash",
                table: "EmailVerifyKeys",
                columns: new[] { "AccountId", "KeyHash" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerifyKeys");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "LockoutUntil",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "VerifyKeyExpiresAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "VerifySendCountDate",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "VerifySendCountToday",
                table: "Account");
        }
    }
}
