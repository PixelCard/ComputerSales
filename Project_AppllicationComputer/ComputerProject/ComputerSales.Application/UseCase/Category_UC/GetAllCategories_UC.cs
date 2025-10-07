using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.UseCaseDTO.Category_DTO;
using ComputerSales.Domain.Entity.ECategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Category_UC
{
    public class GetAllCategories_UC
    {
        private readonly IRespository<Accessories> _repository;

        public GetAllCategories_UC(IRespository<Accessories> repository)
        {
            _repository = repository;
        }

        public async Task<CategoryPagedResult> HandleAsync(CategoryPagedRequest req, CancellationToken ct)
        {
            var pageIndex = req.pageIndex <= 0 ? 1 : req.pageIndex;
            var pageSize = req.pageSize <= 0 ? 10 : req.pageSize;

            // ---- predicate (search theo tên)
            Expression<Func<Accessories, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(req.keyword))
            {
                var kw = req.keyword.Trim();
                predicate = x => x.Name.Contains(kw);
            }


            // ---- orderBy (theo ID tăng dần)
            Func<IQueryable<Accessories>, IOrderedQueryable<Accessories>> orderBy =
                q => q.OrderBy(x => x.AccessoriesID);

            // ---- Tính total 
            var allFiltered = await _repository.ListAsync(
                predicate: predicate,
                orderBy: null,        // không cần sắp xếp khi đếm
                includes: null,
                skip: null, take: null,
                ct: ct
            );

            var total = allFiltered.Count;

            // ---- Lấy page items
            var skip = (pageIndex - 1) * pageSize;
            var pageItems = await _repository.ListAsync(
                predicate: predicate,
                orderBy: orderBy,
                includes: null,
                skip: skip, take: pageSize,
                ct: ct
            );

            var mapped = pageItems.Select(x => x.ToResult()).ToList();

            return new CategoryPagedResult(
                pageIndex,
                pageSize,
                total,
                mapped
            );
        }
    }
}
