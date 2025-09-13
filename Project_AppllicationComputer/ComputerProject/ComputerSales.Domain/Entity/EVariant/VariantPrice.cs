using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EVariant
{
    public enum PriceStatus
    {
        Active,
        Inactive
    }

    public class VariantPrice
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public PriceStatus Status { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }

        public ProductVariant Variant { get; set; }
    }
}
