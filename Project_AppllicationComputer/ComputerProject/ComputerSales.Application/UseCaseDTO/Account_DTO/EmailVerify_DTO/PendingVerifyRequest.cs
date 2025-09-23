using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO.EmailVerify_DTO
{
    public sealed class PendingVerifyRequest
    {
        public int AccountId { get; set; }
        public DateTime VerifyExpiresAtUtc { get; set; }
        public int SecondsLeft =>
            (int)Math.Max(0, (VerifyExpiresAtUtc - DateTime.UtcNow).TotalSeconds);
    }
}
