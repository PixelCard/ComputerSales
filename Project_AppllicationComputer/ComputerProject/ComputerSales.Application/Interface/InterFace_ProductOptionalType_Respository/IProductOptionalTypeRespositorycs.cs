using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository
{
    public interface IProductOptionalTypeRespositorycs
    {
        Task<ProductOptionType?> GetByTwoIdAsync(long productId, int optionalTypeId, CancellationToken ct);
    }
}
