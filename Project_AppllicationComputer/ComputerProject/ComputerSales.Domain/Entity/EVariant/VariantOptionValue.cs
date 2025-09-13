using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EVariant
{
    public class VariantOptionValue
    {
        public int VariantId { get; set; }
        public int OptionalValueId { get; set; }

        public ProductVariant Variant { get; set; }
        public OptionalValue OptionalValue { get; set; }
    }
}
