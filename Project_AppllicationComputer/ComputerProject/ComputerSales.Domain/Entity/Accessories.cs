using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{
    public class Accessories
    {
        public long AccessoriesID { get; set; }
        public string Name { get; set; } = null!;

        public enum AccessoriesStatus
        {
            Inactive = 0,
            Active = 1
        }

        // 1 - N Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
