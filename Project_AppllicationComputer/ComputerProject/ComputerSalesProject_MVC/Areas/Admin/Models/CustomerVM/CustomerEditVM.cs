using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ComputerSalesProject_MVC.Areas.Admin.Models.CustomerVM
{
    public class CustomerEditVM
    {
        [BindNever] public int IDCustomer { get; set; }   // không bind từ form
        [BindNever] public int IDAccount { get; set; }   // không bind từ form

        public string? IMG { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        [Phone] public string? sdt { get; set; }

        [Required, StringLength(300)]
        public string address { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
