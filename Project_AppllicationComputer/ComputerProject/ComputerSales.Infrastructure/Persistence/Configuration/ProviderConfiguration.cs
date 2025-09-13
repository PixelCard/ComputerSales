using ComputerSales.Domain.Entity.EProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> b)
        {
            b.ToTable("Provider");

            b.HasKey(x => x.ProviderID);

            b.Property(x => x.ProviderID)
             .ValueGeneratedOnAdd();

            b.Property(x => x.ProviderName)
             .IsRequired()
             .HasMaxLength(200);
        }
    }
}
