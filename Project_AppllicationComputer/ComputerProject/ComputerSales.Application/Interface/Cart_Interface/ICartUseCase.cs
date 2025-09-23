using ComputerSales.Application.UseCaseDTO.Cart_DTO.Cart_Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Cart_Interface
{
    public interface ICartUseCase
    {
        Task<CartItemsDTO> GetCartPageAsync(int userId, CancellationToken ct = default);
        Task UpdateQuantityAsync(int cartId, int itemId, int qty, CancellationToken ct = default);
        Task AddItemAsync(int userId, int productId, int? productVariantId, int qty, CancellationToken ct = default);
        Task RemoveItemAsync(int cartId, int itemId, CancellationToken ct = default);
        Task AddProtectionPlanAsync(int cartId, int parentItemId, int planVariantId, CancellationToken ct = default);
    }
}
