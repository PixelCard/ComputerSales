namespace ComputerSalesProject_MVC.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public string Title { get; set; } = string.Empty;         // Tên sản phẩm
        public string ImageUrl { get; set; } = string.Empty;      // Link ảnh
        public decimal Price { get; set; }                       // Giá hiện tại
        public decimal OldPrice { get; set; }                    // Giá gốc
        public int DiscountPercent { get; set; }                 // % giảm giá
        public string ShortDescription { get; set; } = string.Empty; // Mô tả ngắn
        public string PromoText { get; set; } = string.Empty;     // Ví dụ: "Free Gift", "Coupon"
        public int Rating { get; set; }                          // số sao (1-5)
        public int ReviewCount { get; set; }                     // số lượng review
        public string Badge { get; set; } = string.Empty;         // ví dụ: "Newegg Select"
    }

}
