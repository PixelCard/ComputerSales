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
        public static Cart Create(int userId, decimal subtotal, decimal discountTotal, decimal shippingFee, int status = 0)
        {
            return new Cart
            {
                UserID = userId,
                Status = status,
                Subtotal = subtotal,
                DiscountTotal = discountTotal,
                ShippingFee = shippingFee,
                GrandTotal = (subtotal - discountTotal) + shippingFee,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };
        }

        // Logic tính lại grand total
        public void RecalculateTotals()
        {
            GrandTotal = (Subtotal - DiscountTotal) + ShippingFee;
            UpdateAt = DateTime.UtcNow;
        }
    }
}
