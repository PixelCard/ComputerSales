using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EPaymentVNPAYTransaction
{
    public class VNPAYPaymentSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N"); // dùng làm vnp_TxnRef
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending/Completed/Failed
        public int? OrderId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int SeqId { get; set; }              // Identity (không phải PK)
        public string TxnRef { get; set; } = "";    // Chuỗi số gửi VNPAY & dùng để tra session
    }
}
