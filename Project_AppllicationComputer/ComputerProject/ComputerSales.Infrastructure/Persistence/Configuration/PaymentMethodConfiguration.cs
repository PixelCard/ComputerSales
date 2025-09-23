using ComputerSales.Domain.Entity.EPayment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> e)
        {
            e.ToTable("PaymentMethod");
            e.HasKey(x => x.PaymentID);
            e.Property(x => x.Code).HasMaxLength(32).IsRequired();
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.HasData(
                new PaymentMethod { PaymentID = 1, Code = "COD", Name = "Cash on Delivery", IsActive = true },
                new PaymentMethod { PaymentID = 2, Code = "ZALOPAY", Name = "ZaloPay", IsActive = true }
            );
        }
    }
}
