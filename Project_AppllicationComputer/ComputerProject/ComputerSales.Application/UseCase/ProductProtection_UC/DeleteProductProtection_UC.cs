using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;

namespace ComputerSales.Application.UseCase.ProductProtection_UC
{
    public class DeleteProductProtection_UC
    {
        private IRespository<ProductProtection> _repoProductProtection;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public DeleteProductProtection_UC(IRespository<ProductProtection> productProctection,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductProtection = productProctection;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductProtectionOutput?> HandleAsync(ProductProtectionDeleteInput input, CancellationToken ct)
        {
            var entity = await _repoProductProtection.GetByIdAsync(input.ProtectionProductId, ct);
            if (entity == null) return null;

            _repoProductProtection.Remove(entity);
            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}




