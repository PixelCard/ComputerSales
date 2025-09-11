using ComputerSales.Domain.Entity.ECustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.E_Order
{
    public class Order
    {
        public int OrderID { get; set; }

      
        public DateTime OrderTime { get; set; }

        public int IDCustomer { get; set; }

        //================ Navigation Property =================//
        public Customer Customer { get; set; }  // FK tới Customer (1 Customer có nhiều Order: 1-N)

        //======================================================//
        public int? PaymentID { get; set; }

    
        public int OrderStatus { get; set; }

        public decimal Subtotal { get; set; }       // tổng trước giảm
        public decimal DiscountTotal { get; set; }  // tổng giảm giá
        public decimal ShippingFee { get; set; }    // phí ship


        //======================================================================================//
        public decimal GrandTotal { get; private set; } // GrandTotal = Subtotal - DiscountTotal + ShippingFee

        //======================================================================================//

        
        public bool Status { get; set; } = true;


        //================ Navigation Property =================//

        // Quan hệ 1 Order có nhiều OrderDetails (1-N) :
        // 1 order có nhiều product , mỗi product sẽ tạo thành 1 order details
        public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();

        //======================================================//




        //================= Factory Method =================//
        public static Order Create(DateTime orderTime, int customerID, int? paymentID, int orderStatus, decimal subtotal, decimal discountTotal, decimal shippingFee)
        {
            var grandTotal = subtotal - discountTotal + shippingFee;
            return new Order
            {
                OrderTime = orderTime,
                IDCustomer = customerID,
                PaymentID = paymentID,
                OrderStatus = orderStatus,
                Subtotal = subtotal,
                DiscountTotal = discountTotal,
                ShippingFee = shippingFee,
                GrandTotal = grandTotal
            };
        }
    }
}
