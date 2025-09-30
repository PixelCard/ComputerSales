using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.EAccount;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class EmailVerifyKeyConfiguration : IEntityTypeConfiguration<EmailVerifyKey>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EmailVerifyKey> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.KeyHash).IsRequired().HasMaxLength(88); 
            b.Property(x => x.ExpiresAt).IsRequired();
            b.Property(x => x.Used).HasDefaultValue(false);
            b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            b.HasIndex(x => new { x.AccountId, x.KeyHash }).IsUnique();
            b.HasOne<Account>()
             .WithMany()
             .HasForeignKey(x => x.AccountId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
