using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Domain.Entity.EProduct;
using ComputerSales.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure.Repositories.Product_Respo
{
    public class ProductRespository : IProductRespository //Xử lý về các nghiệp vụ database
    {
        private readonly AppDbContext _db;

        public ProductRespository(AppDbContext db) => _db = db;

        public async Task AddProduct(Product product, CancellationToken ct) //Vào DB
        {
            await _db.AddAsync(product,ct); //Add -> DB || Add ->  Local 
        }
    }
}
