using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
