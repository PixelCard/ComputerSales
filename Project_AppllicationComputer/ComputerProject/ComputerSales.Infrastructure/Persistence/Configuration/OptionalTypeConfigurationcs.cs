using ComputerSales.Domain.Entity.EOptional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class OptionalTypeConfigurationcs : IEntityTypeConfiguration<OptionType>
    {
        public void Configure(EntityTypeBuilder<OptionType> b)
        {
            b.ToTable("OptionType");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            b.Property(x => x.Code)
             .IsRequired()
             .HasMaxLength(64);

            b.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(200);

            // Code duy nhất trong toàn hệ thống
            b.HasIndex(x => x.Code).IsUnique();

            // 1-N: OptionType - OptionalValues
            b.HasMany(x => x.OptionalValues)
             .WithOne(v => v.OptionType!)
             .HasForeignKey(v => v.OptionTypeId)
             .OnDelete(DeleteBehavior.Cascade); // xóa type -> xóa values
        }
    }
}
