using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.EAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> e)
        {
                //create table 
                e.ToTable("Account");

                //set primary key
                e.HasKey(x => x.IDAccount);              

                //property
                e.Property(x => x.Email).IsRequired().HasMaxLength(150);
                e.Property(x => x.Pass).IsRequired().HasMaxLength(100);

            // IDAccount tự sinh: 'user_' + NEXT VALUE FOR AccountSeq
              e.Property(x => x.IDAccount)
                .HasMaxLength(50)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("('User_' + CAST(NEXT VALUE FOR [AccountSeq] AS varchar(20)))");
            //foureign key 
            e.HasOne(x => x.Role)
                 .WithMany(r => r.Accounts)
                 .HasForeignKey(x => x.IDRole)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

