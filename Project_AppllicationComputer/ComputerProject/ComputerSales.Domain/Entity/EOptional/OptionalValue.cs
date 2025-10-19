using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Domain.Entity.EOptional
{
    public class OptionalValue
    {
        public int Id { get; set; }
        public int OptionTypeId { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
        public decimal  Price { get; set; }

        //1 Optional Type này có thể có trong 1 Optional Values cho 1 product bất kỳ : 1-1 Optional Values
        public OptionType? OptionType { get; set; }
        public ICollection<VariantOptionValue> VariantOptionValues { get; set; }
        

        public static OptionalValue create(int OptionTypeId, string Value, int SortOrder, decimal Price)
        {
            return new OptionalValue
            { OptionTypeId = OptionTypeId, Value = Value, SortOrder = SortOrder,Price = Price };
        }
    }
}
