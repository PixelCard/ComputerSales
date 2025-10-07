namespace ComputerSalesProject_MVC.Areas.Admin.Models.VariantOptionValue
{
    public class OptionValueRowVM
    {
        public long Id { get; set; }
        public long OptionTypeId { get; set; }
        public string Value { get; set; } = "";
        public int SortOrder { get; set; }
        public decimal Price { get; set; }
        public bool IsAssigned { get; set; } = false;
    }
}
