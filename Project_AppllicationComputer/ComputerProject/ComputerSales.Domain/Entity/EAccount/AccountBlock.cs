namespace ComputerSales.Domain.Entity.EAccount
{
    public enum AccountBlockStatus
    {
        Inactive = 0,   // không khóa
        Active = 1      // khóa
    }

    public class AccountBlock
    {
        public int IdBlock { get; set; }
        public int IDAccount { get; set; }
        public DateTime BlockFromUtc { get; set; }
        public DateTime? BlockToUtc { get; set; }

        public bool IsBlock { get; set; } = false;

        public string ReasonBlock { get; set; } = string.Empty;
        public virtual Account Account { get; set; } = null!;

        
        public bool IsActiveNowUtc => DateTime.UtcNow >= BlockFromUtc
                                   && (BlockToUtc == null || DateTime.UtcNow < BlockToUtc.Value);
        public void UpdateStatusByTime()
        {
            IsBlock = IsActiveNowUtc
                ? true
                : false;
        }
        public static AccountBlock Create(int accountId, DateTime fromUtc, DateTime? toUtc, string reason)
        {
            var block = new AccountBlock
            {
                IDAccount = accountId,
                BlockFromUtc = fromUtc,
                BlockToUtc = toUtc,
                ReasonBlock = reason ?? string.Empty
            };
            block.UpdateStatusByTime(); // <--- tự xác định Active/Inactive ngay khi tạo
            return block;
        }
    }
}
