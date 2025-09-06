using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Domain.Entity.ECategory;
using ComputerSales.Domain.Entity.EProvider;

namespace ComputerSales.Domain.Entity.EProduct
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
        public ProductOverview? ProductOverview { get; set; }

        public ICollection<ProductVariant>? ProductVariants { get; set; }

        public ICollection<ProductOptionType> ProductOptionTypes { get; set; } = new List<ProductOptionType>();

        public ProductProtection? ProductProtection { get; set; }
    }
}
