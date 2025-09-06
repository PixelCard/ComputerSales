using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{ 
    // Bảng liên kết Product ↔ OptionType với composite PK (ProductId, OptionTypeId)
     public class ProductOptionType
     {
        public int ProductId { get; set; }
        public int OptionTypeId { get; set; }

        public Product Product { get; set; }
        public OptionType OptionType { get; set; }
     }
}
