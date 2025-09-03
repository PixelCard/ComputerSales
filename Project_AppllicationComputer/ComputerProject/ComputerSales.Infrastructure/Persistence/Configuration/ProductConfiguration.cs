using ComputerSales.Domain.Entity;
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
        public void Configure(EntityTypeBuilder<Product> e)
        {
            //Create Table Product
            e.ToTable(nameof(Product));

            //PK
            e.HasKey(e => e.Id);

            //Indentity
            e.Property(x => x.Id).ValueGeneratedNever();   // quan trọng: KHÔNG dùng UseIdentityColumn

            e.Property(e => e.Name)
            .HasColumnName("user_name")   
            .HasDefaultValue("Không tên") 
            .HasMaxLength(20).HasColumnType("nvarchar");

            e.Property(e => e.Description)
            .HasColumnType("nvarchar(max)");
        }
    }
}
