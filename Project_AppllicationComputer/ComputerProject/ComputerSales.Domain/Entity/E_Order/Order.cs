using ComputerSales.Domain.Entity.ECustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.E_Order
{
    public  class Order
    {
        public int OrderID { get; set; }

        // FK tới Customer (1 Customer có nhiều Order: 1-N)
        public DateTime OrderTime { get; set; }

        //Navigation
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public int? PaymentID { get; set; }

    
        public int OrderStatus { get; set; }

        // Tiền tệ — nên dùng decimal(18,2)
        public decimal Subtotal { get; set; }       // tổng trước giảm
        public decimal DiscountTotal { get; set; }  // tổng giảm giá
        public decimal ShippingFee { get; set; }    // phí ship

        // GrandTotal là cột TÍNH TOÁN: Subtotal - DiscountTotal + ShippingFee
        public decimal GrandTotal { get; private set; }

        // Trạng thái bản ghi (nếu bạn muốn kiểm soát hiển thị/hoạt động)
        public bool Status { get; set; } = true;

        // Quan hệ 1 Order có nhiều OrderDetails (1-N) : 1 order có nhiều product , mỗi product sẽ tạo thành 1 order details
        public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }
}
