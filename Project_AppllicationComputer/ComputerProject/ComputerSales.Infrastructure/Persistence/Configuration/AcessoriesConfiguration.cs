using ComputerSales.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class AcessoriesConfiguration : IEntityTypeConfiguration<Accessories>
    { 
        public void Configure(EntityTypeBuilder<Accessories> b)
        {
            b.ToTable("Accessories");

            b.HasKey(x => x.AccessoriesID);

            b.Property(x => x.AccessoriesID)
             .ValueGeneratedOnAdd();

            b.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(200);
        }
    }
}
