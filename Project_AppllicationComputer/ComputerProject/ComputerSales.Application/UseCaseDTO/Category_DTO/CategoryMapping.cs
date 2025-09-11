using ComputerSales.Domain.Entity.ECategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Category_DTO
{
    public static class CategoryMapping
    {
        public static Accessories ToEntity(this CategoryInput input)
            => Accessories.create(input.name);

        public static CategoryOutput ToResult(this Accessories input)
            => new CategoryOutput(input.AccessoriesID, input.Name);
    }
}
