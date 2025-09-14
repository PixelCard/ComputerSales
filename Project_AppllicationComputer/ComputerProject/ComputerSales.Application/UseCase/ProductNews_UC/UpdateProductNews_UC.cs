using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.ProductNews_DTO;
using ComputerSales.Domain.Entity.ENews;

namespace ComputerSales.Application.UseCase.ProductNews_UC
{
    public class UpdateProductNews_UC
    {
        private readonly IUnitOfWorkApplication _uow;
        private readonly IRespository<ProductNews> _repo;

        public UpdateProductNews_UC(IUnitOfWorkApplication uow, IRespository<ProductNews> repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<ProductNewsOutputDTO?> HandleAsync(ProductNewsOutputDTO input, CancellationToken ct = default)
        {
            // Lấy entity theo ID
            var entity = await _repo.GetByIdAsync(input.ProductNewsID, ct);
            if (entity == null) return null;

            // Cập nhật giá trị
            entity.BlockType = input.BlockType;
            entity.TextContent = input.TextContent;
            entity.ImageUrl = input.ImageUrl;
            entity.Caption = input.Caption;
            entity.DisplayOrder = input.DisplayOrder;
            // entity.CreateDate không nên đổi, nếu muốn thì thêm UpdatedDate

            await _uow.SaveChangesAsync(ct);

            return entity.ToResult();
        }
    }
}
