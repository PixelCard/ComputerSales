using ComputerSales.Domain.Entity.EAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Domain.Entity
{
    public class Account
    {
        public int IDAccount { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }

        public int IDRole { get; set; }

        // navigation propro
        

        //mỗi account chỉ có 1 role
        public Role Role { get; set; }   // hoặc virtual Role Role { get; set; }
    }
}
