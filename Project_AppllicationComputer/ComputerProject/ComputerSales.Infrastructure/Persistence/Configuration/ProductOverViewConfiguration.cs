using ComputerSales.Domain.Entity.EProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class ProductOverViewConfiguration : IEntityTypeConfiguration<ProductOverview>
    {
        public void Configure(EntityTypeBuilder<ProductOverview> b)
        {
            b.ToTable("ProductOverview");

            b.HasKey(x => x.ProductOverviewId);

            b.Property(x => x.ProductOverviewId)
             .ValueGeneratedOnAdd();

            b.Property(x => x.BlockType)
             .HasConversion<int>()  // lưu enum thành int
             .IsRequired();

            b.Property(x => x.TextContent)
             .IsRequired()
             .HasMaxLength(4000);   // cho phép text dài, chỉnh tùy nhu cầu

            b.Property(x => x.ImageUrl)
             .HasMaxLength(1024);

            b.Property(x => x.Caption)
             .HasMaxLength(500);

            b.Property(x => x.DisplayOrder)
             .IsRequired();

            b.Property(x => x.CreateDate)
             .HasDefaultValueSql("GETDATE()");  // default khi insert

            
        }
    }
}
