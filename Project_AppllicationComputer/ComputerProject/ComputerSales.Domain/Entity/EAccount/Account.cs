using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.ECustomer;
using System.Security.Cryptography.X509Certificates;

namespace ComputerSales.Domain.Entity
{
    public class Account
    {
        public int IDAccount { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

        public DateTime CreatedAt { get; set; }

        public int IDRole { get; set; }

        public bool EmailConfirmed { get; private set; }
        public DateTime? VerifyKeyExpiresAt { get; private set; }
        public DateTime? LockoutUntil { get; private set; }
        public int VerifySendCountToday { get; private set; }
        public DateOnly? VerifySendCountDate { get; private set; }

        public void MarkVerifyWindow(DateTime expiresAt) => VerifyKeyExpiresAt = expiresAt;
        public void ConfirmEmail() { EmailConfirmed = true; VerifyKeyExpiresAt = null; }
        public void LockoutFor(TimeSpan span) => LockoutUntil = DateTime.UtcNow.Add(span);
        public bool IsLocked() => LockoutUntil.HasValue && LockoutUntil > DateTime.UtcNow;

        public void BumpSendCount()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (VerifySendCountDate != today) { VerifySendCountDate = today; VerifySendCountToday = 0; }
            VerifySendCountToday++;
        }

        // navigation propro


        //mỗi account chỉ có 1 role
        public Role Role { get; set; }   // hoặc virtual Role Role { get; set; }

        // 1-1
        public Customer? Customer { get; set; }

        // Factory method (nếu bạn muốn áp dụng pattern như Product.Create)

        //1 account có nhiều lần bị khóa
        public virtual ICollection<AccountBlock> AccountBlocks { get; set; } = new List<AccountBlock>();

        //1 account có nhiều lần bị khóa
        public static Account Create(string email, string pass, int idRole,DateTime CreatedAt)
        {
            return new Account
            {
                Email = email,
                Pass = pass,
                IDRole = idRole,
                CreatedAt = CreatedAt
            };
        }
    }
}
