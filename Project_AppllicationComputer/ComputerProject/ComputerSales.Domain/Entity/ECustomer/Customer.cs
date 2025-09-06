using ComputerSales.Domain.Entity.E_Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity.ECustomer
{
    public class   Customer
    {
        public int IDCustomer { get; set; }
        public string? IMG { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        //1 customer có N order : 1-N
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
