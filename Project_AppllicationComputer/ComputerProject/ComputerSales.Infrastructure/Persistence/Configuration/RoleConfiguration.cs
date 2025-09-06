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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            //PK
            builder.HasKey(r => r.IDRole);

            //FK


            //Property
            builder.Property(r => r.TenRole)
                  .IsRequired()
                  .HasMaxLength(100);


            builder.Property(r => r.IDRole).ValueGeneratedOnAdd(); //Tự phát sinh ID


            //RelationShip
        }
    }
}
