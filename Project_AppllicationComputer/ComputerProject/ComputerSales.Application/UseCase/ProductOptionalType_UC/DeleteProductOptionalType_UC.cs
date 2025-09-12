using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.ProductOptionalType_UC
{
    public class DeleteProductOptionalType_UC
    {
        private IUnitOfWorkApplication application;

        private IRespository<ProductOptionType> respository;

        private IProductOptionalTypeRespositorycs productOptionalTypeRespositorycs; 

        public DeleteProductOptionalType_UC(IUnitOfWorkApplication application, 
            IRespository<ProductOptionType> respository, 
            IProductOptionalTypeRespositorycs _productOptionalTypeRespositorycs)
        {
            this.application = application;
            this.respository = respository;
            productOptionalTypeRespositorycs= _productOptionalTypeRespositorycs;
        }


        public async Task<ProductOptionalTypeOutput?> HandleAsync(ProducyOptionalTypeInput
        input, CancellationToken ct)
        {
            ProductOptionType entity =  await productOptionalTypeRespositorycs.GetByTwoIdAsync(
                input.ProductId,
                input.OptionTypeId,
                ct);

            respository.Remove(entity);

             await application.SaveChangesAsync();

            return entity.ToResult();

        }
    }
}
