using ComputerSales.Domain.Entity.E_Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> e)
        {
            // Tên bảng
            e.ToTable("Order");

            // Khóa chính
            e.HasKey(e => e.OrderID);

            e.Property(o => o.OrderID).ValueGeneratedOnAdd();

            // Quan hệ với Customer (1-N)
            e.Property(o => o.IDCustomer).HasColumnName("CustomerID");

            e.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.IDCustomer)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Order_Customer_CustomerID");


            e.Property(o => o.PaymentID).HasColumnName("PaymentID");

            e.HasOne(o => o.Payment)                    
               .WithMany(pm => pm.Orders)                 
               .HasForeignKey(o => o.PaymentID)
               .OnDelete(DeleteBehavior.Restrict)      
               .HasConstraintName("FK_Order_PaymentMethod_PaymentID"); 

            // Thời gian đơn hàng
            e.Property(o => o.OrderTime)
                .HasColumnType("datetime")
                .IsRequired();

            // Trạng thái đơn hàng
            e.Property(o => o.OrderStatus)
                .IsRequired();

            // Tiền tệ
            e.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            e.Property(o => o.DiscountTotal)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            e.Property(o => o.ShippingFee)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // GrandTotal là cột tính toán
            e.Property(o => o.GrandTotal)
                .HasColumnType("decimal(18,2)")
                .HasComputedColumnSql("[Subtotal] - [DiscountTotal] + [ShippingFee]", stored: true);

            // Trạng thái bản ghi
            e.Property(o => o.Status)
                .HasDefaultValue(true);

            e.Property(o => o.OrderNote).HasMaxLength(1000);
        }
    }
}
