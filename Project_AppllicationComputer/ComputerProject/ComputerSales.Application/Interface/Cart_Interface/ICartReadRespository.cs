using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Page;
using ComputerSales.Domain.Entity.ECart;
using ComputerSales.Domain.Entity.EOptional;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Cart_Interface
{
    public interface ICartReadRespository
    {
        Task ClearAsync(int userId, CancellationToken ct);
        Task<Cart?> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<Dictionary<int, ProductVariant>> GetVariantsAsync(int[] variantIds, CancellationToken ct = default);

        Task<OptionalValue?> GetOptionalValueAsync(int id, CancellationToken ct);
    }
}
