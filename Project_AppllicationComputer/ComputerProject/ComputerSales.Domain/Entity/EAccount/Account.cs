using ComputerSales.Domain.Entity.EAccount;
using ComputerSales.Domain.Entity.ECustomer;

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

        // 1-1
        public Customer? Customer { get; set; }   

        // Factory method (nếu bạn muốn áp dụng pattern như Product.Create)


        public static Account Create(string email, string pass, int idRole)
        {
            return new Account
            {
                Email = email,
                Pass = pass,
                IDRole = idRole
            };
        }
    }
}
