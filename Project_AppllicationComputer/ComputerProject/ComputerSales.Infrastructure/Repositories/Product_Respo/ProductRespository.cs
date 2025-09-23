using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ComputerSales.Infrastructure.Repositories.Product_Respo
{
    public class ProductRespository : IProductRespository //Xử lý về các nghiệp vụ database
    {
        private readonly AppDbContext _db;
        public ProductRespository(AppDbContext db) => _db = db;
        public async Task DeleteProductAsync(long productId, byte[]? rowVersion, CancellationToken ct = default)
        {
            // Bỏ filter để tìm cả bản đã đánh dấu xóa
            var entity = await _db.Products
                                  .IgnoreQueryFilters()
                                  .FirstOrDefaultAsync(p => p.ProductID == productId, ct);
            if (entity is null) return;

            if (rowVersion is not null && rowVersion.Length > 0)
            {
                // Concurrency: đặt OriginalValue để EF sinh WHERE RowVersion = @original
                _db.Entry(entity).Property(e => e.RowVersion).OriginalValue = rowVersion;
            }

            _db.Products.Remove(entity);
        }

        public async Task AddProduct(Product product, CancellationToken ct)
        {
            await _db.Products.AddAsync(product, ct);
        }

        public Task UpdateProduct(Product product, CancellationToken ct)
        {
            // Gắn entity với RowVersion để EF kiểm tra concurrency
            _db.Attach(product);
            _db.Entry(product).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public Task<Product> GetProduct(long productId, CancellationToken ct)
        {
            // Include tối thiểu; thêm Include khác nếu cần
            return _db.Products
                      .AsNoTracking() // đọc không track để nhẹ
                      .FirstOrDefaultAsync(p => p.ProductID == productId, ct);
        }
        public async Task<List<Product>> GetAllProductsAsync(CancellationToken ct)
        {
            return await _db.Products
                            .AsNoTracking() // chỉ đọc cho nhẹ
                            .Where(p => !p.IsDeleted) // nếu bạn có soft delete
                            .ToListAsync(ct);
        }

    }
}