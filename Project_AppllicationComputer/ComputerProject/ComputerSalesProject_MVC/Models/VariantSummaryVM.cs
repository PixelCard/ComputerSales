namespace ComputerSalesProject_MVC.Models
{
    public sealed class VariantSummaryVM
    {
        public int VariantId { get; set; }
        public string SKU { get; set; } = "";
        public int Quantity { get; set; }
        public decimal? DisplayPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
    }
}