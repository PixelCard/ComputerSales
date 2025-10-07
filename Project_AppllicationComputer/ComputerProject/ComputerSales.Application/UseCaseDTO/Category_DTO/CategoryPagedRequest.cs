using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Category_DTO
{
    public sealed record CategoryPagedRequest(
            int pageIndex = 1,
            int pageSize = 10,
            string? keyword = null
    );
}
