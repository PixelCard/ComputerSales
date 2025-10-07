using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.VariantOptionValue_DTO;
using ComputerSales.Domain.Entity.EVariant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.VariantOptionValue_UC
{
    public class UpdateVariantOptionValue_UC
    {
        private readonly IRespository<VariantOptionValue> _respository;
        private readonly IUnitOfWorkApplication _unitOfWork;

        public UpdateVariantOptionValue_UC(IRespository<VariantOptionValue> respository, IUnitOfWorkApplication unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<VariantOptionValueOutput_DTO?> HandleAsync(VariantOptionValueInput_DTO input, CancellationToken ct)
        {
            var entity = await _respository.GetByIdAsync((input.VariantId, input.OptionalValueId), ct);
            if (entity == null) return null;

            // update logic nếu có (ở đây 2 field chính là khóa, thường không update)
            entity.VariantId = input.VariantId;
            entity.OptionalValueId = input.OptionalValueId;

            _respository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
