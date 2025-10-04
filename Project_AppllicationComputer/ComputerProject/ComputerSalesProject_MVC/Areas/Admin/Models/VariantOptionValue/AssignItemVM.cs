namespace ComputerSalesProject_MVC.Areas.Admin.Models.VariantOptionValue
{
    public class AssignItemVM
    {
        public long Id { get; set; }
        public string Value { get; set; } = "";
            public decimal Price { get; set; }
        public int SortOrder { get; set; }
        public bool IsAssigned { get; set; }
    }
}
