using ComputerSales.Domain.Entity.EPaymentVNPAYTransaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class VNPAYPaymentTransactionConfiguration : IEntityTypeConfiguration<VNPAYPaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<VNPAYPaymentTransaction> b)
        {
            b.ToTable("VNPAYPaymentTransactions");

            b.HasKey(x => x.Id);

            b.Property(x => x.SessionId)
             .HasMaxLength(32)
             .IsUnicode(false)
             .IsRequired();

            b.Property(x => x.OrderId);

            b.Property(x => x.Gateway)
             .HasMaxLength(32)
             .IsUnicode(false)
             .HasDefaultValue("VNPAY");

            b.Property(x => x.TransactionId)
             .HasMaxLength(64)
             .IsUnicode(false);

            b.Property(x => x.ResponseCode)
             .HasMaxLength(8)
             .IsUnicode(false);

            b.Property(x => x.Amount)
             .HasColumnType("decimal(18,2)")
             .IsRequired();

            b.Property(x => x.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()");

            // FK: SessionId -> VNPAYPaymentSession.Id
            b.HasOne<VNPAYPaymentSession>()
             .WithMany()
             .HasForeignKey(x => x.SessionId)
             .HasPrincipalKey(s => s.Id)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => x.SessionId)
             .HasDatabaseName("IX_VNPAYTrans_SessionId");

            b.HasIndex(x => x.OrderId)
             .HasDatabaseName("IX_VNPAYTrans_OrderId");
        }
    }
}
