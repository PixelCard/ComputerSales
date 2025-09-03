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
using ComputerSales.Application.Interface.UnitOfWorkInterFace;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;

namespace ComputerSales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(
               config.GetConnectionString("Default"),
               sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
           ));
            services.AddScoped<IProductRespository, ProductRespository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
