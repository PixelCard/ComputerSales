using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Domain.Entity.EProduct
{
    // Bảng liên kết Product ↔ OptionType với composite PK (ProductId, OptionTypeId)
    public class ProductOptionType
    {
        public long ProductId { get; set; }
        public int OptionTypeId { get; set; }

        public Product Product { get; set; } = null!;
        public OptionType OptionType { get; set; } = null!;
    }
}
