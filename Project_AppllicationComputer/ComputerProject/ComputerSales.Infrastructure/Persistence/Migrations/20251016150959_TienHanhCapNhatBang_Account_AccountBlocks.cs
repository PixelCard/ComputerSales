using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TienHanhCapNhatBang_Account_AccountBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountBlocks",
                columns: table => new
                {
                    BlockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDAccount = table.Column<int>(type: "int", nullable: false),
                    BlockFromUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlockToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsBlock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReasonBlock = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountBlocks", x => x.BlockId);
                    table.CheckConstraint("CK_AccountBlock_FromTo", "[BlockToUtc] IS NULL OR [BlockToUtc] > [BlockFromUtc]");
                    table.ForeignKey(
                        name: "FK_AccountBlocks_Account_IDAccount",
                        column: x => x.IDAccount,
                        principalTable: "Account",
                        principalColumn: "IDAccount",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountBlocks_IDAccount",
                table: "AccountBlocks",
                column: "IDAccount");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBlocks_IDAccount_BlockFromUtc_BlockToUtc",
                table: "AccountBlocks",
                columns: new[] { "IDAccount", "BlockFromUtc", "BlockToUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountBlocks");
        }
    }
}
