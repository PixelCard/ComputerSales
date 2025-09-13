using ComputerSales.Domain.Entity.ECategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
