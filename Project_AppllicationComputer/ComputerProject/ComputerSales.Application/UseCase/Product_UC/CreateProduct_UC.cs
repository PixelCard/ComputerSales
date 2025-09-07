using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Product_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Product_UC
{
    public class CreateProduct_UC
    {
        private readonly IProductRespository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public CreateProduct_UC(IProductRespository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<ProductOutputDTOcs> HandleAsync(ProductDTOInput input, CancellationToken ct = default)
        {
            // (Tuỳ bạn) thêm validate: ShortDescription not empty, FK tồn tại, v.v.

            Product entity = input.ToEnity(); //Local Storage

            await _repo.AddProduct(entity, ct);

            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
