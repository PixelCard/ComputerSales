using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO;
using ComputerSales.Application.UseCaseDTO.OptionalValue_DTO.UpdateOptionalValue_DTO;
using ComputerSales.Domain.Entity.EOptional;

namespace ComputerSales.Application.UseCase.OptionalValue_UC
{
    public class UpdateOptionalValue_UC
    {
        private IRespository<OptionalValue> respository;

        private IUnitOfWorkApplication unitOfWorkApplication;

        public UpdateOptionalValue_UC(IRespository<OptionalValue> respository,
            IUnitOfWorkApplication unitOfWorkApplication)
        {
            this.respository = respository;
            this.unitOfWorkApplication = unitOfWorkApplication;
        }

        public async Task<OptionalValueOutput?> HandleAsync(UpdateOptionalValueInput input, CancellationToken ct)
        {
            OptionalValue entity = await respository.GetByIdAsync(input.id, ct);

            if (entity == null) return null;

            entity.Value = input.Value;

            entity.SortOrder = input.SortOrder;

            entity.OptionTypeId = input.OptionTypeId;

            respository.Update(entity);

            await unitOfWorkApplication.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
