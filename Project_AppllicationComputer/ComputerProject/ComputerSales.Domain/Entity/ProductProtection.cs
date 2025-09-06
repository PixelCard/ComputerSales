using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
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
    }
}
