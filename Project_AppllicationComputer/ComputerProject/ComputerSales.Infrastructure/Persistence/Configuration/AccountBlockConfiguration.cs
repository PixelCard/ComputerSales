using ComputerSales.Domain.Entity.EAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerSales.Infrastructure.Persistence.Configuration
{
    public class AccountBlockConfiguration : IEntityTypeConfiguration<AccountBlock>
    {
        public void Configure(EntityTypeBuilder<AccountBlock> b)
        {
            b.ToTable("AccountBlocks");

            // Đặt khóa chính tự sinh
            b.HasKey(x => x.BlockId);
            b.Property(x => x.BlockId).ValueGeneratedOnAdd();

            // FK -> Account (int)
            b.Property(x => x.IDAccount).IsRequired();

            // Lưu thời gian tới giờ/phút/giây
            b.Property(x => x.BlockFromUtc)
             .IsRequired()
             .HasColumnType("datetime2");

            b.Property(x => x.BlockToUtc)
             .HasColumnType("datetime2");

            b.Property(x => x.IsBlock)
             .IsRequired()
             .HasDefaultValue(false);

            b.Property(x => x.ReasonBlock)
             .IsRequired()
             .HasMaxLength(500);

            // Ignore computed property
            b.Ignore(x => x.IsActiveNowUtc);

            // THÊM MỚI: Định nghĩa rõ ràng mối quan hệ
            b.HasOne(block => block.Account)         // Mỗi AccountBlock có một Account
             .WithMany(account => account.AccountBlocks) // Mỗi Account có nhiều AccountBlock
             .HasForeignKey(block => block.IDAccount)  // Khóa ngoại là IDAccount
             .OnDelete(DeleteBehavior.Cascade);      // Tùy chọn: Xóa các block khi account bị xóa

            // Indexes
            b.HasIndex(x => x.IDAccount);
            b.HasIndex(x => new { x.IDAccount, x.BlockFromUtc, x.BlockToUtc });

            b.ToTable(tb => tb.HasCheckConstraint(
                "CK_AccountBlock_FromTo",
                "[BlockToUtc] IS NULL OR [BlockToUtc] > [BlockFromUtc]"
));

        }
    }
}