namespace ComputerSales.Domain.Entity.EAccount
{
    public class AccountBlock
    {
        public int BlockId { get; set; }
        public int IDAccount { get; set; }
        public DateTime BlockFromUtc { get; set; } //chỉnh sửa lại allow null để cho phép lưu bản ghi
        public DateTime? BlockToUtc { get; set; }

        //Isblock đổi thành privateset để logic tự tính , không cho người dùng tính 
        public bool IsBlock { get; private set; } = false;

        public string ReasonBlock { get; set; } = string.Empty;

        // Liên kết tới bảng Accounts   
        public virtual Account Account { get; set; } = null!;


        //hàm tính toán xem trạng thái tài khoản có bị block hay không
        public bool IsActiveNowUtc
        {
            get
            {
                var now = DateTime.UtcNow;
                if (now < BlockFromUtc)
                    return false;
                if (BlockToUtc.HasValue && now >= BlockToUtc.Value)
                    return false;
                return true;
            }
        }

        //cập nhật trạng thái IsBlock dựa trên thời gian hiện tại
        public void UpdateStatusByTime()
        {
            IsBlock = IsActiveNowUtc;
        }

        public static AccountBlock Create(int accountId, DateTime fromUtc, DateTime? toUtc, string reason)
        {
            var block = new AccountBlock
            {
                IDAccount = accountId,
                BlockFromUtc = fromUtc,
                BlockToUtc = toUtc,
                ReasonBlock = reason?.Trim() ?? string.Empty
            };
            block.UpdateStatusByTime(); // Tự set IsBlock khi tạo mới
            return block;
        }
    }
}