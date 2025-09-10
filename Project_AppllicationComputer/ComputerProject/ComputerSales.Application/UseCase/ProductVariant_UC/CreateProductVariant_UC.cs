using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.ProductVariant_UC
{
    public class CreateProductVariant_UC
    {
        private IRespository<ProductVariant> _Respository;
        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateProductVariant_UC(IRespository<ProductVariant> respository, IUnitOfWorkApplication unitOfWorkApplication)
        {
            _Respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductVariantOutput> HandleAsync(ProductVariantInput productVariantInput, CancellationToken ct)
        {
            var Enity = productVariantInput.ToEnity();  

            await _Respository.AddAsync(Enity, ct);

            await unitOfWorkApplication.SaveChangesAsync();

            return Enity.ToResult();
        }
    }
}
