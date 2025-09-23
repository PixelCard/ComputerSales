using ComputerSales.Domain.Entity.ECart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class CartPromotionConfiguration : IEntityTypeConfiguration<CartPromotion>
    {
        public void Configure(EntityTypeBuilder<CartPromotion> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.AppliedAmount).HasColumnType("decimal(18,2)");

            // FK: CartPromotion -> Cart
            b.HasOne(x => x.Cart)
             .WithMany(c => c.Promotions)
             .HasForeignKey(x => x.CartID)     // dùng CartID
             .HasPrincipalKey(c => c.ID)       // khóa chính Cart là ID (hoa)
             .OnDelete(DeleteBehavior.Cascade);

            // FK: CartPromotion -> Promotion
            b.HasOne(x => x.Promotion)
             .WithMany()
             .HasForeignKey(x => x.PromotionId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
