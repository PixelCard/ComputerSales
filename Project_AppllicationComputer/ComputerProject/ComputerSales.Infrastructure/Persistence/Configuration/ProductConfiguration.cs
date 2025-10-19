using ComputerSales.Domain.Entity.EProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.ToTable("Product");

            b.HasKey(x => x.ProductID);

            b.Property(x => x.ShortDescription)
             .HasMaxLength(500).IsRequired();

            b.Property(x => x.SKU)
             .HasMaxLength(64).IsRequired();

            b.Property(x => x.Slug)
             .HasMaxLength(180).IsRequired();

            b.Property(x => x.Status)
             .HasConversion<string>()               // lưu “Active/Inactive” cho dễ đọc (hoặc int nếu bạn thích)
             .HasMaxLength(16).IsRequired();

            b.Property(x => x.RowVersion)
             .IsRowVersion();

            b.HasIndex(x => x.SKU).IsUnique();
            b.HasIndex(x => x.Slug).IsUnique();
            b.HasIndex(x => new { x.ProviderID, x.AccessoriesID, x.Status });

            // 1–n
            b.HasMany(x => x.ProductVariants)
             .WithOne(v => v.Product)
             .HasForeignKey(v => v.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

           //1-n
            b.HasMany(p => p.ProductOptionTypes)
             .WithOne(pot => pot.Product)
             .HasForeignKey(pot => pot.ProductId);

            // 1–1 Overview
            b.HasOne (p => p.ProductOverviews) // 1 Product có NHIỀU 1 overview
             .WithOne(o => o.Product)          // 1 Overview thuộc về 1 Product
             .HasForeignKey<ProductOverview>(p => p.ProductId) // Khóa ngoại là ProductId trên bảng Overview
             .OnDelete(DeleteBehavior.Cascade);

            // 1–1 Protection
            b.HasOne(x => x.ProductProtection)
             .WithOne(p => p.Product)
             .HasForeignKey<ProductProtection>(p => p.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK cơ bản
            b.HasOne(x => x.Accessories)
             .WithMany(c => c.Products)
             .HasForeignKey(x => x.AccessoriesID);

            b.HasOne(x => x.Provider)
             .WithMany(br => br.Products)
             .HasForeignKey(x => x.ProviderID);
        }
    }
}
