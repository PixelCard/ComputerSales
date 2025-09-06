using ComputerSales.Domain.Entity.EProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    internal class ProductOptionTypeConfiguration : IEntityTypeConfiguration<ProductOptionType>
    {
        public void Configure(EntityTypeBuilder<ProductOptionType> b)
        {
            b.ToTable("ProductOptionType");

            // Khóa chính tổng hợp để đảm bảo 1 OptionType không gán trùng cho cùng 1 Product
            b.HasKey(x => new { x.ProductId, x.OptionTypeId });

            // FK -> Product (N-1)
            b.HasOne(x => x.Product)
             .WithMany(p => p.ProductOptionTypes)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK -> OptionType (N-1)
            b.HasOne(x => x.OptionType)
             .WithMany(o => o.ProductOptionTypes)
             .HasForeignKey(x => x.OptionTypeId)
             .OnDelete(DeleteBehavior.Cascade);

            // (tùy chọn) Index để query nhanh theo OptionType
            b.HasIndex(x => x.OptionTypeId);
        }
    }
}
