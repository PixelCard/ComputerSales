using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.AccountBlocks
{
    public sealed class AccountBlockCreateVM
    {
        [Required]
        public int IDAccount { get; set; }

        [Required]
        public DateTime BlockFromUtc { get; set; }

        public DateTime? BlockToUtc { get; set; }

        [MaxLength(500)]
        public string? ReasonBlock { get; set; }
    }
}
