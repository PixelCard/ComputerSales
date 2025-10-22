using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Models
{
    public class CustomerEditVM_Customer
    {
        [Required]
        public int IDCustomer { get; set; }

        [Required]
        public int IDAccount { get; set; }

        public string? IMG { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Phone]
        public string? sdt { get; set; }

        [Required, StringLength(300)]
        public string address { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }  // ngày sinh

        public IFormFile? AvatarFile { get; set; }
    }
}
