namespace ComputerSalesProject_MVC.Models
{
    public sealed class OverviewBlockVM
    {
        public long ProductOverviewId { get; set; }  // đổi về long cho thống nhất
        public OverviewBlockType BlockType { get; set; }
        public string? TextContent { get; set; }     // cho phép null
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateDate { get; set; }
    }

}
