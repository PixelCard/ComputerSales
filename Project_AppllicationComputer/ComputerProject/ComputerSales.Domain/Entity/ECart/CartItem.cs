namespace ComputerSales.Domain.Entity.ECart
{
    public class CartItem
    {
        public int ID { get; set; }
        public int CartID { get; set; }
        public int? ProductID { get; set; }
        public int? ProductVariantID { get; set; }
        public int? ParentItemID { get; set; }

        public int ItemType { get; set; }         // có thể map enum sau
        public string SKU { get; set; }
        public string Name { get; set; }
        public string OptionSummary { get; set; }
        public string ImageUrl { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int? PerItemLimit { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsSelected { get; set; }

        // Navigation
        public Cart Cart { get; set; }
        public CartItem ParentItem { get; set; }
        public ICollection<CartItem> Children { get; set; } = new List<CartItem>();
    }
}
