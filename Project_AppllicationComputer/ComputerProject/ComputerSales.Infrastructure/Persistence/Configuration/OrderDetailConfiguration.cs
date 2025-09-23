using ComputerSales.Domain.Entity.E_Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> e)
        {
            e.ToTable("OrderDetails");

            // primary key + khóa kết hợp
            e.HasKey(x => new { x.OrderID, x.ProductVariantID });

            // Quan hệ Order (1 - N)
            e.HasOne(x => x.Order)
             .WithMany(o => o.Details ) 
             .HasForeignKey(x => x.OrderID)
             .OnDelete(DeleteBehavior.Cascade);

          
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductID).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.ProductVariant).WithMany().HasForeignKey(x => x.ProductVariantID).OnDelete(DeleteBehavior.Restrict);

            //property

            //giá cả
            e.Property(x => x.Quantity)
             .IsRequired();

            e.Property(x => x.UnitPrice)
             .HasColumnType("decimal(18,2)")
             .IsRequired();

            e.Property(x => x.Discount)
             .HasColumnType("decimal(18,2)")
             .HasDefaultValue(0)
             .IsRequired();

            // Computed column cho TotalPrice
            e.Property(x => x.TotalPrice)
             .HasColumnType("decimal(18,2)")

             //tự động tính sau đó input vào totalprice
             .HasComputedColumnSql("[UnitPrice] - [Discount] * [Quantity]", stored: true);

            e.Property(x => x.SKU)
             .HasMaxLength(100);

            e.Property(x => x.Name)
             .HasMaxLength(255);

            e.Property(x => x.OptionSummary)
             .HasMaxLength(255);

            e.Property(x => x.ImageUrl)
             .HasMaxLength(500);

            // Index gợi ý
            e.HasIndex(x => x.ProductID);
            e.HasIndex(x => x.ProductVariantID);
            e.HasIndex(x => x.SKU);
        }
    } 
}
