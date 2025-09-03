using ComputerSales.Application.Interface.ProductInterFace;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Infrastructure.Repositories.Product_Respo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComputerSales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration _)
        {
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ComputerSalesDb"));
            services.AddScoped<IProductRespository, ProductRespository>();
            return services;
        }
    }
}
