using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{
    public class Provider
    {
        public long ProviderID { get; set; }
        public string ProviderName { get; set; } = null!;

        // 1 - N Product
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
