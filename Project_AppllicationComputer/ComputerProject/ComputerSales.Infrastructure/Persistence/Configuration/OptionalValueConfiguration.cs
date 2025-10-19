using ComputerSales.Domain.Entity.EOptional;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class OptionalValueConfiguration : IEntityTypeConfiguration<OptionalValue>
    {
        public void Configure(EntityTypeBuilder<OptionalValue> b)
        {
            b.ToTable("OptionalValue");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            b.Property(x => x.OptionTypeId)
             .IsRequired();

            b.Property(x => x.Value)
             .IsRequired()
             .HasMaxLength(200);

            b.Property(x => x.SortOrder)
             .HasDefaultValue(0)
             .IsRequired();

            b.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            // Đảm bảo 1 OptionType không có 2 value trùng tên
            b.HasIndex(x => new { x.OptionTypeId, x.Value })
             .IsUnique();

            // N-1: OptionalValue -> OptionType
            b.HasOne(x => x.OptionType)
             .WithMany(t => t.OptionalValues!)
             .HasForeignKey(x => x.OptionTypeId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
