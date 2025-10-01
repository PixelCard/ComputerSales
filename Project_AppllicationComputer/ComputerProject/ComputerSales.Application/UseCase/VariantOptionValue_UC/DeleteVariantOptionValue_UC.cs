using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO.DeleteVariantOptionValue;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantOptionValue_UC
{
    public class DeleteVariantOptionValue_UC
    {
        private readonly IRespository<VariantOptionValue> _respository;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public DeleteVariantOptionValue_UC(IRespository<VariantOptionValue> respository, IUnitOfWorkApplication unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleAsync(DeleteVariantOptionalValue_Input input, CancellationToken ct)
        {
            var entity = await _respository.GetByIdAsync(input.VariantOptionValue, ct);
            if (entity == null) return false;

            _respository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(ct);
            return true;
        }
    }
}
