using ComputerSales.Domain.Entity.ECustomer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> e)
        {
            e.ToTable("Customer");
            e.HasKey(c => c.CustomerID);
            e.Property(c => c.CustomerID).HasColumnName("IDCustomer");
            e.Property(x => x.IMG).HasMaxLength(255);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Date).HasColumnType("datetime");
            e.Property(x => x.address).IsRequired().HasMaxLength(1000);
            e.Property(x => x.sdt).HasMaxLength(12);

            // 1-1: Customer -> Account (FK duy nhất IDAccount)
            e.HasOne(x => x.Account)
             .WithOne(a => a.Customer)
             .HasForeignKey<Customer>(x => x.IDAccount)  // tạo FK ở bảng Customer
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.IDAccount).IsUnique();      // đảm bảo 1-1
        }
    }
}
