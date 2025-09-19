using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Domain.Entity.ECart;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.Cart_Respo.CartWrite
{
    public class CartWriteRepository : ICartWriteRepository
    {
        private readonly AppDbContext _db;

        public CartWriteRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<Cart?> GetByIdAsync(int cartId, CancellationToken ct = default)
             => _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.ID == cartId, ct);
    }
}
