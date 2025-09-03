using ComputerSales.Application.Interface.ProductInterFace;
using ComputerSales.Domain.Entity;
using ComputerSales.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.Product_Respo
{
    public class ProductRespository : IProductRespository
    {
        private readonly AppDbContext appDbContext;

        public ProductRespository(AppDbContext appDbContext) => this.appDbContext = appDbContext;

        public async Task CreateProduct(Product product,CancellationToken ct) => 
            await appDbContext.Products.AddAsync(product, ct);

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) 
            => await appDbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id,ct);
    }
}
