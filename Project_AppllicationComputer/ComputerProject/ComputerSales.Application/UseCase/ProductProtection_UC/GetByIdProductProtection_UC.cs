using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO;
using ComputerSales.Application.UseCaseDTO.ProductProtection_DTO.GetByIdDTO;

namespace ComputerSales.Application.UseCase.ProductProtection_UC
{
    public class GetByIdProductProtection_UC
    {
        private IRespository<ProductProtection> _repoProductProtection;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public GetByIdProductProtection_UC(IRespository<ProductProtection> productProtection,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductProtection = productProtection;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductProtectionOutput?> HandleAsync(ProductProtectionGetByIDInput input, CancellationToken ct)
        {
            var entity = await _repoProductProtection.GetByIdAsync(input.ProtectionProductId, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
