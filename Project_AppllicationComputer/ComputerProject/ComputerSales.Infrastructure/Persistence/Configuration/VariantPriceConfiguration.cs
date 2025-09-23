using ComputerSales.Domain.Entity.EVariant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class VariantPriceConfiguration : IEntityTypeConfiguration<VariantPrice>
    {
        public void Configure(EntityTypeBuilder<VariantPrice> b)
        {
            b.ToTable("VariantPrice");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            b.Property(x => x.VariantId)
             .IsRequired();

            b.Property(x => x.Currency)
             .IsRequired()
             .HasMaxLength(10); // hoặc 3 nếu dùng ISO-4217

            b.Property(x => x.Price)
             .HasPrecision(18, 2)
             .IsRequired();

            b.Property(x => x.DiscountPrice)
             .HasPrecision(18, 2)
             .HasDefaultValue(0);

            b.Property(x => x.Status)
             .HasConversion<int>()
             .IsRequired();

            b.Property(x => x.ValidFrom);
            b.Property(x => x.ValidTo);

            // N-1: VariantPrice -> ProductVariant
            b.HasOne(x => x.Variant)
             .WithMany(v => v.VariantPrices)
             .HasForeignKey(x => x.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            // Index hay dùng để truy vấn giá theo thời gian & trạng thái
            b.HasIndex(x => x.VariantId);

            b.HasCheckConstraint("CK_VariantPrice_FromBeforeTo", "[EffectiveFrom] < [EffectiveTo]");


            b.HasIndex(x => new { x.VariantId, x.Status, x.ValidFrom, x.ValidTo });

            // Index truy vấn nhanh
            b.HasIndex(x => new { x.VariantId, x.Currency, x.Status, x.ValidFrom, x.ValidTo });
        }
    }
}
