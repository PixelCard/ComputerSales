using System.ComponentModel.DataAnnotations;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.Product_Overview
{
    public class ProductOverviewCreateVM
    {
        [Required]
        public long ProductId { get; set; }

        [Required]
        [Display(Name = "Block Type")]
        public OverviewBlockType BlockType { get; set; } = OverviewBlockType.Text;

        [Display(Name = "Text Content")]
        public string? TextContent { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Caption")]
        public string? Caption { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be greater than 0")]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 1;
    }
}
