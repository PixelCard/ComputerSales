using ComputerSales.Application.Interface.ProductInterFace;
using ComputerSales.Application.UseCaseDTO.Product.GetByID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.Product.GetByID
{
    public class GetProductByIdUseCase
    {
        private readonly IProductRespository respository;

        public GetProductByIdUseCase(IProductRespository respository) =>  this.respository = respository;

        //Dùng để thực thi Async để chạy qua các tầng Infrastructure mà Hàm này gọi + Domain để giải quyết các validation
        public async Task<ProductDto?> ExcuteAsync(GetProductByIdInput input,CancellationToken ct) 
        {
            var p = await respository.GetByIdAsync(input.Id, ct);

            return p is null ? null : new ProductDto(p.Id, p.Name, p.Description); 
        }
    }
}
