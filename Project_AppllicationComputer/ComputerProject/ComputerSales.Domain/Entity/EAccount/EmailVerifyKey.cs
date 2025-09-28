namespace ComputerSales.Domain.Entity.EAccount
{
    public class EmailVerifyKey
    {
        public Guid Id { get; set; }
        public int AccountId { get; set; }
        public string KeyHash { get; set; } = default!; // SHA-256(Base64Url(key))
        public DateTime ExpiresAt { get; set; }    //1p của ngày hiện tại
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
