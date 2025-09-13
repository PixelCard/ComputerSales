namespace ComputerSales.Domain.Entity.EAccount
{
    public class Role
    {
        public int IDRole { get; set; }
        public string TenRole { get; set; }

        // Quan hệ 1 Role có nhiều Account (1-N)
        // Một Role (Admin/User/Staff) sẽ được gán cho nhiều Account
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        public static Role Create(string tenRole) => new Role
        {
            TenRole = tenRole
        };

    }
}
