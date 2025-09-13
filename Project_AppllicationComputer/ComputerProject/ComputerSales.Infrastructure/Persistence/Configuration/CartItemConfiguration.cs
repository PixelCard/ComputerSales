using ComputerSales.Domain.Entity.ECart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItem");


            //Primary key 
            builder.HasKey(x => x.ID);


            //Property 
            builder.Property(x => x.CartID).IsRequired();

            builder.Property(x => x.ItemType).IsRequired();

            builder.Property(x => x.SKU)
                   .HasMaxLength(100);

            builder.Property(x => x.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(x => x.OptionSummary)
                   .HasMaxLength(500);

            builder.Property(x => x.ImageUrl)
                   .HasMaxLength(500);

            builder.Property(x => x.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .HasDefaultValue(0m);

            builder.Property(x => x.Quantity)
                   .HasDefaultValue(1);

            builder.Property(x => x.PerItemLimit);

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsSelected)
                   .HasDefaultValue(true);

            // Self-reference: ParentItem (dùng cho bundle/combo)
            //builder.HasOne(x => x.ParentItem)
            //       .WithMany(p => p.Children)
            //       .HasForeignKey(x => x.ParentItemID)
            //       .OnDelete(DeleteBehavior.Restrict);

            // FK tới Cart
            builder.HasOne(x => x.Cart)
                   .WithMany(c => c.Items)
                   .HasForeignKey(x => x.CartID)
                   .OnDelete(DeleteBehavior.Cascade);

            // Index gợi ý
            builder.HasIndex(x => x.CartID).HasDatabaseName("IX_CartItem_CartID");
            builder.HasIndex(x => x.ProductVariantID).HasDatabaseName("IX_CartItem_ProductVariantID");
            builder.HasIndex(x => x.ParentItemID).HasDatabaseName("IX_CartItem_ParentItemID");
            builder.HasIndex(x => x.SKU).HasDatabaseName("IX_CartItem_SKU");

            // Check constraints
            builder.HasCheckConstraint("CK_CartItem_Quantity_Positive", "[Quantity] > 0")
                   .HasCheckConstraint("CK_CartItem_UnitPrice_NonNegative", "[UnitPrice] >= 0");
        }
    }
    
}
