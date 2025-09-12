using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.OptionalValue_UC
{
    public class CreateOptionalValue_UC
    {
        private IRespository<OptionalValue> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateOptionalValue_UC(IRespository<OptionalValue> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalValueOutput?> HandleAsync(OptionalValueInput input, CancellationToken ct)
        {
            OptionalValue entity = input.ToEnity();

            await respository.AddAsync(entity, ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
