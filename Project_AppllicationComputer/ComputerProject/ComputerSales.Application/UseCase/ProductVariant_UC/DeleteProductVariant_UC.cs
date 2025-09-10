using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO;
using ComputerSales.Application.UseCaseDTO.ProductVariant_DTO.DeleteDTO_ProductVariant;

namespace ComputerSales.Application.UseCase.ProductVariant_UC
{
    public class DeleteProductVariant_UC
    {
        private IRespository<ProductVariant> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public DeleteProductVariant_UC(IRespository<ProductVariant> repo, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo = repo;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductVariantOutput?> HandleAsync(ProductVariantDelete_DTO input,CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.Id, ct);
            if (entity == null) return null;

            _repo.Remove(entity);
            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}







