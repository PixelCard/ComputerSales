using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Product_Interface
{
    public interface IProductRespository
    {
        Task AddProduct(Product product, CancellationToken ct);
        
        Task UpdateProduct(Product product, CancellationToken ct);

        Task DeleteProductAsync(long productId, byte[]? rowVersion, CancellationToken ct = default);

        Task<Product> GetProduct(long productId, CancellationToken ct);
    }
}
