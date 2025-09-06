using ComputerSales.Domain.Entity.ECart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");


            //Primary key
            builder.HasKey(x => x.ID);


            //Property
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(0);

            builder.Property(x => x.Subtotal)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);

            builder.Property(x => x.DiscountTotal)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);

            builder.Property(x => x.ShippingFee)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);

            // GrandTotal = Subtotal - DiscountTotal + ShippingFee (stored computed column)
            builder.Property(x => x.GrandTotal)
                   .HasColumnType("decimal(18,2)")
                   .HasComputedColumnSql("[Subtotal]-[DiscountTotal]+[ShippingFee]", stored: true);

            builder.Property(x => x.ExpiresAT);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdateAt)
                   .HasDefaultValueSql("GETUTCDATE()");


            //Foureign key
            // 1 Cart - N CartItems
            builder.HasMany(x => x.Items)
                   .WithOne(i => i.Cart)
                   .HasForeignKey(i => i.CartID)
                   .OnDelete(DeleteBehavior.Cascade);


            //------------------------------------------------------------
            // Index gợi ý để truy vấn nhanh
            builder.HasIndex(x => new { x.UserID, x.Status })
                   .HasDatabaseName("IX_Cart_User_Status");

            // Các ràng buộc kiểm tra hợp lệ giá trị tiền
            builder.HasCheckConstraint("CK_Cart_Subtotal_NonNegative", "[Subtotal] >= 0")
                   .HasCheckConstraint("CK_Cart_Discount_NonNegative", "[DiscountTotal] >= 0")
                   .HasCheckConstraint("CK_Cart_Shipping_NonNegative", "[ShippingFee] >= 0");
        }
    }
    
}
