using ComputerSales.Domain.Entity.E_Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerSales.Domain.Entity.ECustomer
{
    public class Customer
    {
        [Column("IDCustomer")]
        public int CustomerID { get; set; }
        public string? IMG { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? sdt { get; set; }

        public string address { get; set; }

        public DateTime Date { get; set; } //ngày sinh


        // FK duy nhất trỏ sang Account (ràng buộc 1-1)
        public int IDAccount { get; set; }
        public Account Account { get; set; } = default!;

        //1 customer có N order : 1-N
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public static Customer create(string? img, string name, string? description,string address,string? sdt, DateTime date, int idAccount)
            => new Customer { 
                IMG = img, 
                Name = name, 
                address = address,
                sdt = sdt,
                Description = description, 
                Date = date, 
                IDAccount = idAccount };
    }
}
