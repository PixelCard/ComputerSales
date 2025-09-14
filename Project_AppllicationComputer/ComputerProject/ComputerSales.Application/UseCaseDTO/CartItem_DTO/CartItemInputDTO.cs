using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.CartItem_DTO
{
    public sealed record CartItemInputDTO(
        int CartID,
        int? ProductID,
        int? ProductVariantID,
        int? ParentItemID,
        int ItemType,
        string SKU,
        string Name,
        string OptionSummary,
        string ImageUrl,
        decimal UnitPrice,
        int Quantity,
        int? PerItemLimit,
        bool IsSelected);
}



