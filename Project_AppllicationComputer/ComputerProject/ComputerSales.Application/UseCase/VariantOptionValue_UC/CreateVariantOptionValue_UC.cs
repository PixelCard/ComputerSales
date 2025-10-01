using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO;
using ComputerSales.Domain.Entity.EVariant;

namespace ComputerSales.Application.UseCase.VariantOptionValue_UC
{
    public class CreateVariantOptionValue_UC
    {
        private readonly IRespository<VariantOptionValue> _respository;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public CreateVariantOptionValue_UC(IRespository<VariantOptionValue> respository, IUnitOfWorkApplication unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<VariantOptionValueOutput_DTO?> HandleAsync(VariantOptionValueInput_DTO input, CancellationToken ct)
        {
            var entity = input.ToEnity();
            await _respository.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return entity.ToResult();
        }
    }
}
