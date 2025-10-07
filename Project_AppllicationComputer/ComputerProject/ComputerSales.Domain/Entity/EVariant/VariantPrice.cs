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
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public ProductVariant Variant { get; set; }
        public static VariantPrice Create(
         int VariantId,
         string Currency,
         decimal Price,
         decimal DiscountPrice,
         PriceStatus Status,
         DateTime? ValidFrom,
         DateTime? ValidTo )
        {
            return new VariantPrice()
            {
                VariantId = VariantId,
                Currency = Currency,
                Price = Price,
                DiscountPrice = DiscountPrice,
                Status = Status,
                ValidFrom = ValidFrom,
                ValidTo = ValidTo
            };
        }   
    }
}
