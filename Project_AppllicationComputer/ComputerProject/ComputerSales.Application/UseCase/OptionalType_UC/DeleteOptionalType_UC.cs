using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalType_DTO.DeleteOptionalType_DTO;
using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Application.UseCase.OptionalType_UC
{
    public class DeleteOptionalType_UC
    {
        private IRespository<OptionType> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public DeleteOptionalType_UC(IRespository<OptionType> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalTypeOutput?> HandleAsync(DeleteOptionalTypeInput input, CancellationToken ct)
        {
            OptionType entity = await respository.GetByIdAsync(input.OptionalTypeID,ct);

            if (entity == null) return null;

            respository.Remove(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
