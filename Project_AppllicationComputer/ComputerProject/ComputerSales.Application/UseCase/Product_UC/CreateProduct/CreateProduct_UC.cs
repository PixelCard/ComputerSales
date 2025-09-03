using ComputerSales.Application.Interface.ProductInterFace;
using ComputerSales.Application.Interface.UnitOfWorkInterFace;
using ComputerSales.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ComputerSales.Application.UseCaseDTO.ProductDTO.CreateProduct.CreateProductDTO;

namespace ComputerSales.Application.UseCase.ProductUC.CreateProduct
{
    public class CreateProduct_UC
    {
        private readonly IProductRespository _repo;
        private readonly IUnitOfWork _uow;

        public CreateProduct_UC(IProductRespository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<CreateProductResult> ExecuteAsync(CreateProductInput input, CancellationToken ct = default)
        {
            var entity = Product.Create(input.Name, input.Description); 
            await _repo.CreateProduct(entity, ct);                           
            await _uow.SaveChangesAsync(ct);                           
            return new CreateProductResult(entity.Id);
        }
    }
}
