using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.ERefreshToken
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime Expires { get; set; }
        public DateTime? RevokedAt { get; set; }   // thời điểm bị revoke
        public bool IsActive => RevokedAt == null && DateTime.UtcNow < Expires;
        public Account Account { get; set; } = default!;
    }
}
