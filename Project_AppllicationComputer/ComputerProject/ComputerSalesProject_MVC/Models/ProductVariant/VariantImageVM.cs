namespace ComputerSalesProject_MVC.Models.ProductVariant
{
    public class VariantImageVM
    {
        public int Id { get; set; }
        public string Url { get; set; } = "";
        public int SortOrder { get; set; }
        public string? DescriptionImg { get; set; }
    }
}
