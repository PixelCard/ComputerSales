using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCaseDTO.Product_DTO.DeleteProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Product_UC
{
    public class DeleteProduct_UC
    {
        private readonly IProductRespository _repo;
        private readonly IUnitOfWorkApplication _uow;

        public DeleteProduct_UC(IProductRespository repo, IUnitOfWorkApplication uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<bool> HandleAsync(DeleteProductInput input, CancellationToken ct = default)
        {
            byte[]? rv = null;

            if (!string.IsNullOrWhiteSpace(input.RowVersionBase64))
                rv = Convert.FromBase64String(input.RowVersionBase64!);

            await _repo.DeleteProductAsync(input.ProductID, rv, ct);

            // nếu không tìm thấy entity, repo có thể không làm gì; coi như delete "không thành công"
            var changes = await _uow.SaveChangesAsync(ct);
            return changes > 0;
        }
    }
}
