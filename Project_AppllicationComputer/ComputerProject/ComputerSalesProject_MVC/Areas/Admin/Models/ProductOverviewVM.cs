using System.ComponentModel.DataAnnotations;

public sealed class ProductOverviewVM
{
    [Required] public string BlockType { get; set; } = "Text"; // Text/List/Image/Logo/Table
    [Required] public string TextContent { get; set; } = "";
    public string? ImageUrl { get; set; }
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; } = 1;
}