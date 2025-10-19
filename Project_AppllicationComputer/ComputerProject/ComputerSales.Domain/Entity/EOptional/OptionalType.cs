using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSales.Domain.Entity.EOptional
{
    public class OptionType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        // 1-N : 1 Optional Type - N Optional Value
        public ICollection<OptionalValue>? OptionalValues { get; set; }
        public ICollection<ProductOptionType> ProductOptionTypes { get; set; }

        public static OptionType create(string code,string name) =>
            new OptionType { Code = code, Name = name };
    }
}
