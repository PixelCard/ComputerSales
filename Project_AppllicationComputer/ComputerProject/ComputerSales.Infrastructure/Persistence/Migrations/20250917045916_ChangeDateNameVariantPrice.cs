using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerSales.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateNameVariantPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) DROP CHECK constraint phụ thuộc
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.check_constraints 
           WHERE [name] = N'CK_VariantPrice_FromBeforeTo' 
             AND parent_object_id = OBJECT_ID(N'[VariantPrice]'))
  ALTER TABLE [VariantPrice] DROP CONSTRAINT [CK_VariantPrice_FromBeforeTo];
");

            // 2) DROP default constraints (nếu có) trên 2 cột
            migrationBuilder.Sql(@"
DECLARE @df1 sysname, @df2 sysname;
SELECT @df1 = d.name
FROM sys.default_constraints d
JOIN sys.columns c ON c.default_object_id = d.object_id
WHERE d.parent_object_id = OBJECT_ID(N'[VariantPrice]') AND c.name = N'EffectiveFrom';
IF @df1 IS NOT NULL EXEC('ALTER TABLE [VariantPrice] DROP CONSTRAINT [' + @df1 + ']');

SELECT @df2 = d.name
FROM sys.default_constraints d
JOIN sys.columns c ON c.default_object_id = d.object_id
WHERE d.parent_object_id = OBJECT_ID(N'[VariantPrice]') AND c.name = N'EffectiveTo';
IF @df2 IS NOT NULL EXEC('ALTER TABLE [VariantPrice] DROP CONSTRAINT [' + @df2 + ']');
");

            // 3) RENAME cột (khuyên dùng) hoặc DROP/ADD tuỳ bạn
            migrationBuilder.Sql("EXEC sp_rename 'VariantPrice.EffectiveFrom', 'ValidFrom', 'COLUMN';");
            migrationBuilder.Sql("EXEC sp_rename 'VariantPrice.EffectiveTo',   'ValidTo',   'COLUMN';");

            // 4) Tạo lại CHECK constraint theo tên mới (tùy quy tắc của bạn)
            migrationBuilder.Sql(@"
ALTER TABLE [VariantPrice]
ADD CONSTRAINT [CK_VariantPrice_ValidFromBeforeTo]
CHECK ([ValidFrom] IS NULL OR [ValidTo] IS NULL OR [ValidFrom] <= [ValidTo]);
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert constraint
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.check_constraints 
           WHERE [name] = N'CK_VariantPrice_ValidFromBeforeTo' 
             AND parent_object_id = OBJECT_ID(N'[VariantPrice]'))
  ALTER TABLE [VariantPrice] DROP CONSTRAINT [CK_VariantPrice_ValidFromBeforeTo];
");

            migrationBuilder.Sql("EXEC sp_rename 'VariantPrice.ValidFrom', 'EffectiveFrom', 'COLUMN';");
            migrationBuilder.Sql("EXEC sp_rename 'VariantPrice.ValidTo',   'EffectiveTo',   'COLUMN';");

            migrationBuilder.Sql(@"
ALTER TABLE [VariantPrice]
ADD CONSTRAINT [CK_VariantPrice_FromBeforeTo]
CHECK ([EffectiveFrom] IS NULL OR [EffectiveTo] IS NULL OR [EffectiveFrom] <= [EffectiveTo]);
");
        }
    }
}

