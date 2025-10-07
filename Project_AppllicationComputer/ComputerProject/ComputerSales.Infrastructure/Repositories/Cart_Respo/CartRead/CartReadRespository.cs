using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Domain.Entity.ECart;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.Cart_Respo.CartRead
{
    public class CartReadRespository : ICartReadRespository
    {
        private readonly AppDbContext _db;

        public CartReadRespository(AppDbContext db)
        {
            _db = db;
        }

        public async Task ClearAsync(int userId, CancellationToken ct)
        {
            await _db.CartItems
            .Where(x => x.Cart.UserID == userId)
            .ExecuteDeleteAsync(ct);
        }

        public Task<Cart?> GetByUserAsync(int userId, CancellationToken ct = default)
            => _db.Carts.Include(c => c.Items)  // lấy Cart + Items (CartItem có Parent/Children sẵn) :                       
                    .FirstOrDefaultAsync(c => c.UserID == userId, ct);

        public async Task<OptionalValue?> GetOptionalValueAsync(int id, CancellationToken ct)
        {
            return await _db.optionalValues
             .Include(x => x.OptionType)   // để lấy tên loại: OptionalType.Name
             .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Dictionary<int, ProductVariant>> GetVariantsAsync(int[] variantIds, CancellationToken ct = default)
        {
            var list = await _db.productVariants
            .Where(v => variantIds.Contains(v.Id))
            .Include(v => v.VariantPrices)
            .Include(v => v.VariantImages)
            .Include(v => v.VariantOptionValues)
                .ThenInclude(vov => vov.OptionalValue!)
                    .ThenInclude(ov => ov.OptionType!)
            .ToListAsync(ct);
            return list.ToDictionary(v => v.Id);
        }



    }
}
