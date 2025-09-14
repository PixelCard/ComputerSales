using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.E_Order
{
    public class OrderDetail
    {

        //Primary key 
        public int OrderID { get; set; }
        public long? ProductID { get; set; }
        public int ProductVariantID { get; set; } // biến thể ( có thể xem là thể loại ) 
        public int Quantity { get; set; }

        //giá gốc
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        //giá bán
        public decimal TotalPrice { get; private set; }


        // có thể xem như là mã ISO
        public string SKU { get; set; }
        public string Name { get; set; }
        public string OptionSummary { get; set; }
        public string ImageUrl { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public static OrderDetail Create(
              int orderId,
              long? productId,
              int productVariantId,
              int quantity,
              decimal unitPrice,
              decimal discount,
              string sku,
              string name,
              string optionSummary,
              string imageUrl)
        {
            return new OrderDetail
            {
                OrderID = orderId,
                ProductID = productId,
                ProductVariantID = productVariantId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                TotalPrice = (unitPrice - discount) * quantity,
                SKU = sku,
                Name = name,
                OptionSummary = optionSummary,
                ImageUrl = imageUrl
            };
        }
    }
}
