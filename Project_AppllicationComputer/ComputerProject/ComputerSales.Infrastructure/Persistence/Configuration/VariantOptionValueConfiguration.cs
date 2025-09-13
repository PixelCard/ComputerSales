using ComputerSales.Domain.Entity.EVariant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class VariantOptionValueConfiguration : IEntityTypeConfiguration<VariantOptionValue>
    {
        public void Configure(EntityTypeBuilder<VariantOptionValue> b)
        {
            b.ToTable("VariantOptionValue");

            // PK tổng hợp để 1 OptionalValue không bị gán trùng cho cùng 1 Variant
            b.HasKey(x => new { x.VariantId, x.OptionalValueId });

            b.Property(x => x.VariantId).IsRequired();
            b.Property(x => x.OptionalValueId).IsRequired();

            // FK -> ProductVariant
            b.HasOne(x => x.Variant)
             .WithMany(v => v.VariantOptionValues)
             .HasForeignKey(x => x.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            // FK -> OptionalValue
            b.HasOne(x => x.OptionalValue)
             .WithMany(ov => ov.VariantOptionValues)
             .HasForeignKey(x => x.OptionalValueId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.OptionalValueId);
        }
    }
}
