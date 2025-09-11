using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO.GetByIdDTO;
using ComputerSales.Application.UseCaseDTO.ProductOverView_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.ProductOptionalType_DTO;

namespace ComputerSales.Application.UseCase.ProductOptionalType_UC
{
    public class GetByIdProductOptionalType_UC
    {
        private IProductOptionalTypeRespositorycs _respository;
        private IUnitOfWorkApplication unitOfWorkApplication;

        public GetByIdProductOptionalType_UC(IProductOptionalTypeRespositorycs respository, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            _respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<ProductOptionalTypeOutput?> HandleAsync(ProducyOptionalTypeInput input, CancellationToken ct)
        {
            var entity = await _respository.GetByTwoIdAsync(input.ProductId,input.OptionTypeId, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
