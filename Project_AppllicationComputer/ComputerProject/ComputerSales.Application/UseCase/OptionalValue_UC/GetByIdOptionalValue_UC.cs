using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.UpdateOptionalValue_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.GetByIdOptionalValue_DTO;

namespace ComputerSales.Application.UseCase.OptionalValue_UC
{
    public class GetByIdOptionalValue_UC
    {
        private IRespository<OptionalValue> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public GetByIdOptionalValue_UC(IRespository<OptionalValue> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalValueOutput?> HandleAsync(GetByIdOptionalValueInput input, CancellationToken ct)
        {
            OptionalValue entity = await respository.GetByIdAsync(input.Id, ct);

            if (entity == null) return null;

            return entity.ToResult();
        }
    }
}
