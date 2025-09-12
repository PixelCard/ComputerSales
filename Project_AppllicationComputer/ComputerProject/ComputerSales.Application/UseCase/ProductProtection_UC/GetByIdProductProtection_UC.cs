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
using ComputerSales.Domain.Entity.E_Order;

namespace ComputerSales.Application.UseCase.ProductProtection_UC
{
    public class GetByIdProductProtection_UC
    {
        private IRespository<ProductProtection> _repo;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public GetByIdProductProtection_UC(IRespository<ProductProtection> productProtectionOverView,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repo   = productProtectionOverView;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductProtectionOutput?> HandleAsync(ProductProtectionGetByIDInput input, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(input.ProtectionProductId, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
