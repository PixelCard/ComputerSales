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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.ToTable("Product");

            b.HasKey(x => x.ProductID);

            b.Property(x => x.ProductID)
             .ValueGeneratedOnAdd();

            b.Property(x => x.ShortDescription)
             .IsRequired()
             .HasMaxLength(500);

            b.Property(x => x.Status)
             .HasConversion<int>()  // lưu enum thành int
             .IsRequired();

            // 1-N: Accessories — Products 
            b.HasOne(x => x.Accessories)
             .WithMany(a => a.Products)
             .HasForeignKey(x => x.AccessoriesID)
             .OnDelete(DeleteBehavior.Restrict); // tránh xóa dây chuyền nếu không muốn

            // 1-N: Provider — Product 
            b.HasOne(x => x.Provider)
             .WithMany(p => p.Products)
             .HasForeignKey(x => x.ProviderID)
             .OnDelete(DeleteBehavior.Restrict);

            // unique index để đảm bảo 1-1
            b.HasIndex(x => x.ProviderID)
             .IsUnique();
        }
    }
}
