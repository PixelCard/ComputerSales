using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSalesProject_MVC.Models
{
    public sealed class PriceRowVM
    {
        public int Id { get; set; }
        public string Currency { get; set; } = "$";
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public PriceStatus Status { get; set; }          // enum PriceStatus thay vì int
        public DateTime? ValidFrom { get; set; }         // nullable cho khớp DB
        public DateTime? ValidTo { get; set; }
    }
}
