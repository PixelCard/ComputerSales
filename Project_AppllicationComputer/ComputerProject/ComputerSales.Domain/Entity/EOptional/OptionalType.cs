using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EOptional
{
    //Các các loại option cho các Optional Value cần thiết (-- 'model', 'edition')
    public class OptionalType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        // 1 Optional Type có thể có hoặc không có 1 OptionalValues nào cả
        // nhưng mà
        // 1 Optional Type có thể chứa nhiều Optional Values cho 1 product đó

        // 1-N : 1 Optional Type - N Optional Value
        public ICollection<OptionalValue>? OptionalValues { get; set; }
        public ICollection<ProductOptionType> ProductOptionTypes { get; set; }

        public static OptionalType create(string code,string name) =>
            new OptionalType { Code = code, Name = name };
    }
}
