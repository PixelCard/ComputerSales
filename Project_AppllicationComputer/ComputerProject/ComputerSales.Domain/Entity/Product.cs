using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{
    public enum ProductStatus
    {
        Inactive = 0,
        Active = 1
    }
    public class Product
    {
        private Product() { } // EF cần
        public long ProductID { get; set; }
        public string ShortDescription { get; set; } = null!;
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        // FKs
        public long AccessoriesID { get; set; }
         public long ProviderID { get; set; }

         // Navigations
         public Accessories Accessories { get; set; } = null!;
         public Provider Provider { get; set; } = null!;
    }
}
