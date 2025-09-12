using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.OptionalType_UC
{
    public class CreateOptionalType_UC
    {
        private IRespository<OptionType> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public CreateOptionalType_UC(IRespository<OptionType> respository, 
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalTypeOutput?> HandleAsync(OptionalTypeInput input,CancellationToken ct)
        {
            OptionType entity = input.ToEnity();

            await respository.AddAsync(entity,ct);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
