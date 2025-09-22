namespace ComputerSalesProject_MVC.Models
{
    public sealed class OverviewBlockVM
    {
        public int ProductOverviewId { get; set; }
        public OverviewBlockType BlockType { get; set; }
        public string TextContent { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string? Caption { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateDate { get; set; }
    }

}
