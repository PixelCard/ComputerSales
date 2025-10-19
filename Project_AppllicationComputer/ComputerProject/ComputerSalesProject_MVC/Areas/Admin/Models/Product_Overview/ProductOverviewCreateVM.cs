using System.ComponentModel.DataAnnotations;
using ComputerSales.Domain.Entity.EProduct;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.Product_Overview
{
    public class ProductOverviewCreateVM
    {
        [Required]
        public long ProductId { get; set; }

        [Display(Name = "Text Content")]
        public string? TextContent { get; set; }
    }
}
