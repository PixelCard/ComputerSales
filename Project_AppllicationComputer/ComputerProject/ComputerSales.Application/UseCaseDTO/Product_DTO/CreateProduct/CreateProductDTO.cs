using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.ProductDTO.CreateProduct
{
    public class CreateProductDTO
    {
        public sealed record CreateProductInput(string Name, string Description);
        public sealed record CreateProductResult(Guid Id);
    }
}
