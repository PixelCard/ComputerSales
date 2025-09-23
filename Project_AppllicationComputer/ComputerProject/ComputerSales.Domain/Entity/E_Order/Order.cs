using ComputerSales.Domain.Entity.ECustomer;
using ComputerSales.Domain.Entity.EPayment;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSales.Domain.Entity.E_Order
{
    public enum OrderStatus
    {
        ChoXacNhan = 1, // Mới đặt, chờ shop xác nhận
        DangDongGoi = 2, // Đang chuẩn bị/đóng gói
        DangGiao = 3, // Đang giao hàng
        DaGiaoThanhCong = 4, // Giao thành công
        DaHuy = 5  // Đã hủy
    }


    public class Order
    {
        public int OrderID { get; set; }

      
        public DateTime OrderTime { get; set; }

        public int IDCustomer { get; set; }

        //================ Navigation Property =================//
        public Customer Customer { get; set; }  // FK tới Customer (1 Customer có nhiều Order: 1-N)

        //======================================================//
        public int? PaymentID { get; set; }

        public string? OrderNote {  get; set; }
    
        public OrderStatus OrderStatus { get; set; }

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


        [ForeignKey(nameof(PaymentID))]              
        public PaymentMethod? Payment { get; set; }  

        //======================================================//




        //================= Factory Method =================//
        public static Order Create(DateTime orderTime, int customerID, int? paymentID, OrderStatus orderStatus,string? OrderNote, decimal subtotal, decimal discountTotal, decimal shippingFee)
        {
            var grandTotal = subtotal - discountTotal + shippingFee;
            return new Order
            {
                OrderTime = orderTime,
                IDCustomer = customerID,
                PaymentID = paymentID,
                OrderStatus = orderStatus,
                OrderNote = OrderNote,
                Subtotal = subtotal,
                DiscountTotal = discountTotal,
                ShippingFee = shippingFee,
                GrandTotal = grandTotal
            };
        }
    }
}
