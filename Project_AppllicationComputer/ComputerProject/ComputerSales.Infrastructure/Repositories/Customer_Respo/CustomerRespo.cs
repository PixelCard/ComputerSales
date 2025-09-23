using ComputerSales.Application.Interface.InterfaceCustomerRespo;
using ComputerSales.Domain.Entity.ECustomer;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.Customer_Respo
{
    public class CustomerRespo : ICustomerRespo
    {
        private AppDbContext db;

        public CustomerRespo(AppDbContext db)
        {
            this.db = db;
        }

        public Task<Customer?> GetCustomerByUserID(int userID, CancellationToken ct)
        {
            return db.Customers
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.IDAccount == userID, ct);    
        }
    }
}
