using ComputerSales.Domain.Entity.EProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class ProductProtectionConfiguration : IEntityTypeConfiguration<ProductProtection>
    {
        public void Configure(EntityTypeBuilder<ProductProtection> b)
        {
            b.ToTable("ProductProtection");

            b.HasKey(x => x.ProtectionProductId);

            b.Property(x => x.ProtectionProductId)
             .ValueGeneratedOnAdd();

            b.Property(x => x.DateBuy)
             .IsRequired();

            b.Property(x => x.DateEnd)
             .IsRequired();

            b.Property(x => x.Status)
             .HasConversion<int>()   // enum → int
             .IsRequired();

            // 1-1: Product - ProductProtection
            b.HasOne(x => x.Product)
             .WithOne(p => p.ProductProtection)  
             .HasForeignKey<ProductProtection>(x => x.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
