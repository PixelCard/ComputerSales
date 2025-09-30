using ComputerSales.Domain.Entity.EProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> b)
        {
            b.ToTable("ProductVariant");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            b.Property(x => x.SKU)
             .IsRequired()
             .HasMaxLength(100);

            b.Property(x => x.Status)
             .HasConversion<int>()   // enum -> int
             .IsRequired();

            b.Property(x => x.Quantity)
             .IsRequired();


            b.Property(x => x.VariantName).IsRequired().HasMaxLength(1000);

            // 1-N: Product -> ProductVariants
            b.HasOne(x => x.Product)
             .WithMany(p => p.ProductVariants)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            // 1-N: ProductVariant -> VariantOptionValues
            b.HasMany(x => x.VariantOptionValues)
             .WithOne(v => v.Variant)
             .HasForeignKey(v => v.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            // 1-N: ProductVariant -> VariantPrices
            b.HasMany(x => x.VariantPrices)
             .WithOne(v => v.Variant)
             .HasForeignKey(v => v.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            // 1-N: ProductVariant -> VariantImages
            b.HasMany(x => x.VariantImages)
             .WithOne(v => v.Variant)
             .HasForeignKey(v => v.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            // Index để query nhanh theo Product
            b.HasIndex(x => x.ProductId);
        }
    }
}
