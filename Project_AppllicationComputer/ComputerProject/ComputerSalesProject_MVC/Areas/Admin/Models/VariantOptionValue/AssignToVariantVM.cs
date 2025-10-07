namespace ComputerSalesProject_MVC.Areas.Admin.Models.VariantOptionValue
{
    public class AssignToVariantVM
    {
        public long VariantId { get; set; }
        public long OptionTypeId { get; set; }
        public List<AssignItemVM> Items { get; set; } = new();
    }
}
