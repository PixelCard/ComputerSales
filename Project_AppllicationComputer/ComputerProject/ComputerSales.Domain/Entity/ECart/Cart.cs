namespace ComputerSales.Domain.Entity.ECart
{
    public class Cart
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int Status { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal GrandTotal { get; private set; } // computed

        public DateTime? ExpiresAT { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        //1-N
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
