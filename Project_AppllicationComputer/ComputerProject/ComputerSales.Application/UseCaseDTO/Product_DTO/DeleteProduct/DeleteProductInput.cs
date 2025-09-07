using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Product_DTO.DeleteProduct
{
    /// <summary>
    /// Xoá mềm sản phẩm. RowVersionBase64 có thể null nếu bạn không cần concurrency khi delete.
    /// </summary>
    public sealed record DeleteProductInput(
        long ProductID,
        string? RowVersionBase64   // optional
    );
}
