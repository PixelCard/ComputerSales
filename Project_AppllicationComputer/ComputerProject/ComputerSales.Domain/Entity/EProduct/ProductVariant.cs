using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EProduct
{
    public enum VariantStatus
    {
        Active,
        Inactive
    }

    public class ProductVariant
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public string SKU { get; set; }
        public VariantStatus Status { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public ICollection<VariantOptionValue> VariantOptionValues { get; set; }
        public ICollection<VariantPrice> VariantPrices { get; set; }
        public ICollection<VariantImage> VariantImages { get; set; }
    }
}
