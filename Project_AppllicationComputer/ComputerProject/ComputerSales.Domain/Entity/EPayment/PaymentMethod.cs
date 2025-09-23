using ComputerSales.Domain.Entity.E_Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EPayment
{
    public class PaymentMethod
    {
        // Dùng tên cột "PaymentID" để khớp FK hiện có trong Order
        public int PaymentID { get; set; }
        public string Code { get; set; } = null!;   // ví dụ: "COD", "ZALOPAY"
        public string Name { get; set; } = null!;   // ví dụ: "Cash on Delivery", "ZaloPay"
        public bool IsActive { get; set; } = true;

        // Navigation (tuỳ chọn)
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
