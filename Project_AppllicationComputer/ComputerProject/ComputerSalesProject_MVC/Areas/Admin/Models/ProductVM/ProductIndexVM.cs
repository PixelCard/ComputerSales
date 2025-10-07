namespace ComputerSalesProject_MVC.Areas.Admin.Models.ProductVM
{
    public class ProductIndexVM
    {
        public List<ProductRowVM> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string? Query { get; set; }
        public string? Status { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / Math.Max(1, PageSize));
    }
}
