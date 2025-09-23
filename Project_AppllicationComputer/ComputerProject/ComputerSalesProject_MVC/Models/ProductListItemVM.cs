
namespace ComputerSalesProject_MVC.Models
{
    // Models/ProductListItemVM.cs
    public sealed class ProductListItemVM
    {
        public int ProductId { get; set; }
        public string SKU { get; set; } = "";
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public string ImageUrl { get; set; } = "/images/placeholder.svg";

        public string Currency { get; set; } = "$";
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }

        // Tuỳ chọn
        public int? VariantCount { get; set; }
        public bool IsNew { get; set; } = true;
        public bool FreeShip { get; set; } = false;
        public double? Rating { get; set; }      // 0..5
        public int? RatingCount { get; set; }
    }

}
