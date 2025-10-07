using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSalesProject_MVC.Models
{
    public class VariantPriceVM
    {
        public int Id { get; set; }
        public int VariantId { get; set; }          // FK -> ProductVariant.Id
        public string Currency { get; set; } = "VND";
        public decimal Price { get; set; }          // giá gốc
        public decimal? DiscountPrice { get; set; } // giá KM (<= Price)
        public PriceStatus Status { get; set; }     // Active/Inactive
        public DateTime? ValidFrom { get; set; }    // UTC
        public DateTime? ValidTo { get; set; }      // UTC

        public ProductVariant Variant { get; set; } = null!;
    }
}
