using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EVariant
{
    public class VariantImage
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public string DescriptionImg { get; set; }

        public ProductVariant Variant { get; set; }
    }
}
