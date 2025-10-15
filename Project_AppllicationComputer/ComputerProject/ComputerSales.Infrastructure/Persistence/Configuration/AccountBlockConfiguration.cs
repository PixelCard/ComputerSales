//using ComputerSales.Domain.Entity;
//using ComputerSales.Domain.Entity.EAccount;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace ComputerSales.Infrastructure.Persistence.Configuration
//{
//    public class AccountBlockConfiguration : IEntityTypeConfiguration<AccountBlock>
//    {
//        public void Configure(EntityTypeBuilder<AccountBlock> b)
//        {
//            b.ToTable("AccountBlocks");

//            // PK
//            b.HasKey(x => x.IdBlock);
//            b.Property(x => x.IdBlock)
//             .ValueGeneratedOnAdd();

//            // Columns
//            b.Property(x => x.IDAccount)
//             .IsRequired();

//            // Lưu tới GIÂY (đổi (0)->(3)/(7) nếu cần mili/100ns)
//            b.Property(x => x.BlockFromUtc)
//             .IsRequired()
//             .HasColumnType("datetime2");

//            b.Property(x => x.BlockToUtc)
//             .HasColumnType("datetime2");

//            b.Property(x => x.IsBlock)
//             .IsRequired()
//             .HasDefaultValue(false);

//            b.Property(x => x.ReasonBlock)
//             .IsRequired()
//             .HasMaxLength(500);

//            // Không map helper
//            b.Ignore(x => x.IsActiveNowUtc);

//            // Quan hệ: nhiều block cho 1 account (giữ đơn giản giống file bạn)
//            b.HasOne(x => x.Account)
//             .WithMany() // nếu sau này thêm ICollection<AccountBlock> trong Account => đổi .WithMany(a => a.AccountBlocks)
//             .HasForeignKey(x => x.IDAccount)
//             .OnDelete(DeleteBehavior.Cascade);

//            // Index cơ bản
//            b.HasIndex(x => x.IDAccount);
//            b.HasIndex(x => new { x.IDAccount, x.BlockFromUtc, x.BlockToUtc });
//        }
//    }
//}
