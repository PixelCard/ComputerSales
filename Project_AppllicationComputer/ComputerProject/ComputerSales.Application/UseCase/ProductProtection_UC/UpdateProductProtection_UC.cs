using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.UpdateDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.UpdateDTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;

namespace ComputerSales.Application.UseCase.ProductProtection_UC
{
    public class UpdateProductProtection_UC
    {
        private IRespository<ProductProtection> _repoProductProtection;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public UpdateProductProtection_UC(IRespository<ProductProtection> repoProductProtection,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductProtection = repoProductProtection;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductProtectionOutput?> HandleAsync(ProductProtectionUpdateInput input, CancellationToken ct)
        {
            var entity = await _repoProductProtection.GetByIdAsync(input.ProtectionProductId, ct);

            if (entity == null) return null;

            entity.DateBuy = input.DateBuy;

            entity.DateEnd = input.DateEnd;

            _repoProductProtection.Update(entity);

            await _unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
