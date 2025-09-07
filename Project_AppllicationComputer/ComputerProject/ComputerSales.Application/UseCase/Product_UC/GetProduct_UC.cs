using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Application.UseCaseDTO.Product_DTO.GetByID;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Product_UC
{
    public class GetProduct_UC
    {
        private readonly IProductRespository _repo;

        public GetProduct_UC(IProductRespository repo) => _repo = repo;

        public async Task<ProductOutputDTOcs?> HandleAsync(ProductGetByIDInput input, CancellationToken ct = default)
        {
            var entity = await _repo.GetProduct(input.productID, ct);
            if (entity is null) return null;
            return entity.ToResult();
        }
    }
}
