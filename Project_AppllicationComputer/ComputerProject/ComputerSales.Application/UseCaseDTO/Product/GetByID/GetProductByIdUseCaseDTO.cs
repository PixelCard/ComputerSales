using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Product.GetByID
{
    public sealed record GetProductByIdInput(Guid Id);
    public sealed record ProductDto(Guid Id, string Name, string Description);
}
