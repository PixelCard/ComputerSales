using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.DeleteDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using ComputerSales.Domain.Entity.EProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;

namespace ComputerSales.Application.UseCase.ProductOvetView_UC
{
    public class GetByIdProductOverView_UC
    {
        private IRespository<ProductOverview> _repoProductOverView;
        private IUnitOfWorkApplication _unitOfWorkApplication;

        public GetByIdProductOverView_UC(IRespository<ProductOverview> productOverView,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _repoProductOverView = productOverView;
            _unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductOverViewOutput?> HandleAsync(GetByIDProductOverViewInput input, CancellationToken ct)
        {
            var entity = await _repoProductOverView.GetByIdAsync(input.ProductOverViewID, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
