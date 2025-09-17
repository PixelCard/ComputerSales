using ComputerSales.Domain.Entity.ECart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Cart_Interface
{
    public interface ICartWriteRepository
    {
        Task<Cart?> GetByIdAsync(int cartId, CancellationToken ct = default);
    }
}
