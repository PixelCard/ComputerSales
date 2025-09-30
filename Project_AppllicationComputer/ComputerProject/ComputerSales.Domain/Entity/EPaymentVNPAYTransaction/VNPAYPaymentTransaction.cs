using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EPaymentVNPAYTransaction
{
    public class VNPAYPaymentTransaction
    {
        public int Id { get; set; }                           // PK (identity)
        public string SessionId { get; set; } = default!;     // FK -> VNPAYPaymentSession.Id (vnp_TxnRef)
        public int? OrderId { get; set; }                     // Map vào đơn nếu có
        public string Gateway { get; set; } = "VNPAY";        // "VNPAY"
        public string TransactionId { get; set; } = string.Empty; // vnp_TransactionNo
        public string ResponseCode { get; set; } = string.Empty;  // vnp_ResponseCode (00/0 = OK)
        public decimal Amount { get; set; }                   // số tiền (đơn vị bình thường)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
