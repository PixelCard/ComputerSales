namespace ComputerSalesProject_MVC.Models.ProductVariant
{
    public sealed class VariantSummaryVM
    {
        public int VariantId { get; set; }
        public string SKU { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }           // giá hiện hành (đã resolve)
        public decimal? OldPrice { get; set; }
        public string VariantName { get; set; }
    }
}