using ComputerSales.Domain.Entity.EVariant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class VariantImgConfiguration : IEntityTypeConfiguration<VariantImage>
    {
        public void Configure(EntityTypeBuilder<VariantImage> b)
        {
            b.ToTable("VariantImage");

            b.HasKey(x => x.Id);

            b.Property(x => x.Id)
             .ValueGeneratedOnAdd();

            b.Property(x => x.VariantId)
             .IsRequired();

            b.Property(x => x.Url)
             .IsRequired()
             .HasMaxLength(1024);

            b.Property(x => x.SortOrder)
             .HasDefaultValue(0)
             .IsRequired();

            b.Property(x => x.DescriptionImg)
             .HasMaxLength(500);

            // N-1: VariantImage -> ProductVariant
            b.HasOne(x => x.Variant)
             .WithMany(v => v.VariantImages)
             .HasForeignKey(x => x.VariantId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => new { x.VariantId, x.SortOrder });
        }
    }
}
