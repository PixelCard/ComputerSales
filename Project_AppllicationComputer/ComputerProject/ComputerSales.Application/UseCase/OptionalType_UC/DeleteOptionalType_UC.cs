using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO.DeleteOptionalType_DTO;
using ComputerSales.Domain.Entity.EOptional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.OptionalType_UC
{
    public class DeleteOptionalType_UC
    {
        private IRespository<OptionalType> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteOptionalType_UC(IRespository<OptionalType> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalTypeOutput?> HandleAsync(DeleteOptionalTypeInput input, CancellationToken ct)
        {
            OptionalType entity = await respository.GetByIdAsync(input.OptionalTypeID,ct);

            if (entity == null) return null;

            respository.Remove(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
