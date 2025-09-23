namespace ComputerSalesProject_MVC.Models
{
    public sealed class SearchResultVM
    {
        public string Query { get; set; } = "";
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<SearchResultItemVM> Items { get; set; } = Array.Empty<SearchResultItemVM>();
    }
}
