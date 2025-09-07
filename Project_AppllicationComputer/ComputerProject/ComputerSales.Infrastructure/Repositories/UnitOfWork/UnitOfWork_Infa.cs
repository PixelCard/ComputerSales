using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork_Infa : IUnitOfWorkApplication
    {
        private readonly AppDbContext _db;

        public UnitOfWork_Infa(AppDbContext db) => _db = db;
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct); //Sau khi gọi hàm này thì data mới vào db
    }
}
