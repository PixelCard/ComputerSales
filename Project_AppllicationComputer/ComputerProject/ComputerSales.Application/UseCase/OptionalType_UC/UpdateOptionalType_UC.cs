using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO.UpdateOptionalType_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.OptionalType_UC
{
    public class UpdateOptionalType_UC
    {
        private IRespository<OptionType> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public UpdateOptionalType_UC(IRespository<OptionType> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalTypeOutput?> HandleAsync(UpdateOptionalTypeInput input, CancellationToken ct)
        {
            OptionType entity = await respository.GetByIdAsync(input.id, ct);

            if (entity == null)
            {
                return null;
            }

            entity.Name = input.Name;

            entity.Code = input.Code;

            respository.Update(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
