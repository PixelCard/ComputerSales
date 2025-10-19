namespace ComputerSalesProject_MVC.Models
{
    public sealed class OverviewBlockVM
    {
        public long ProductOverviewId { get; set; }  // đổi về long cho thống nhất
        public string? TextContent { get; set; }     // cho phép null
    }

}
