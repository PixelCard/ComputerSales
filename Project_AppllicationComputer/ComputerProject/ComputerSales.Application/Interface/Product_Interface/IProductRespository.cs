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
    }
}
