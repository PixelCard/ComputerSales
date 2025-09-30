using ComputerSales.Domain.Entity.EPaymentVNPAYTransaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class VNPAYPaymentSessionConfiguration : IEntityTypeConfiguration<VNPAYPaymentSession>
    {
        public void Configure(EntityTypeBuilder<VNPAYPaymentSession> b)
        {
            b.ToTable("VNPAYPaymentSessions");

            b.HasKey(x => x.Id);
            b.Property(x => x.Id)
             .HasMaxLength(32)             // Guid "N" = 32 ký tự
             .IsUnicode(false);

            b.Property(x => x.UserId)
             .IsRequired();

            b.Property(x => x.Amount)
             .HasColumnType("decimal(18,2)")
             .IsRequired();

            b.Property(x => x.Status)
             .HasMaxLength(20)
             .IsUnicode(false)
             .HasDefaultValue("Pending");

            b.Property(x => x.OrderId);

            b.Property(x => x.CreatedAt)
             .HasDefaultValueSql("GETUTCDATE()");

            // Index để tra cứu nhanh phiên theo User + thời gian
            b.HasIndex(x => new { x.UserId, x.CreatedAt })
             .HasDatabaseName("IX_VNPAYSession_UserId_CreatedAt");

            b.Property(x => x.SeqId)
             .ValueGeneratedOnAdd(); // identity cho non-PK

            b.Property(x => x.TxnRef)
             .HasMaxLength(20)
             .IsUnicode(false)
             .IsRequired(false);     // <-- cho phép NULL ban đầu

            b.HasIndex(x => x.TxnRef)
             .IsUnique()
             .HasDatabaseName("UX_VNPAYSession_TxnRef")
             .HasFilter("[TxnRef] IS NOT NULL"); // <-- chỉ unique khi có giá trị
        }
    }
}
