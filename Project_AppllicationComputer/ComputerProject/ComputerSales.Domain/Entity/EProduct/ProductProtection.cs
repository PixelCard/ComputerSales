using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.EProduct
{
    public enum WarrantyStatus
    {
        UnderWarranty,    // Bảo hành
        NoWarranty,       // Không bảo hành
        WarrantyExpired   // Ngưng bảo hành (sau 15 ngày kể từ DateEnd)
    }

    public class ProductProtection
    {
        public long ProtectionProductId { get; set; }
        public DateTime DateBuy { get; set; }
        public DateTime DateEnd { get; set; }
        public WarrantyStatus Status { get; set; }

        //FK:1-1 to product
        public long ProductId { get; set; }
        public Product? Product { get; set; }

        public static ProductProtection create(long ProductId, DateTime DateBuy, DateTime DateEnd, WarrantyStatus Status)
            => new ProductProtection
            {
                ProductId = ProductId,
                DateBuy=DateBuy,
                DateEnd=DateEnd,
                Status=Status
            };
    }
}
