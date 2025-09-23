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

        public async Task AddItemAsync(CartItem item, CancellationToken ct = default)
        {
            // Phòng hờ: đảm bảo CreatedAt/IsSelected có giá trị hợp lý
            if (item.CreatedAt == default) item.CreatedAt = DateTime.UtcNow;

            _db.CartItems.Add(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default)
        {
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync(ct);
            return cart;
        }

        public Task<Cart?> GetByIdAsync(int cartId, CancellationToken ct = default)
             => _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.ID == cartId, ct);
    }
}
