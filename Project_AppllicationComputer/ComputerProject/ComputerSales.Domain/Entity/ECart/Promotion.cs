using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.ECart
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = "";
        public bool IsPercentage { get; set; }   // true: %; false: số tiền
        public decimal Value { get; set; }       // 10 -> 10% hoặc 10$
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
