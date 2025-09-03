using ComputerSales.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.ProductInterFace
{
    public interface IProductRespository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
