using ComputerSales.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Infrastructure.Repositories.Role_Respo;
using Microsoft.Identity.Client;
using ComputerSales.Infrastructure.Repositories.Account_Respo;
using ComputerSales.Application.Interface.Account_Interface;

namespace ComputerSales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(
               config.GetConnectionString("Toan"),
               sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));
            services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();
            services.AddScoped<IProductRespository, IProductRespository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountRepository, AccountRespository>();
            return services;
        }
    }
}


//Rồi mới tới lệnh này

//Mỗi lần muốn chạy migration vào CMD chạy cậu lệnh này:

//dotnet ef database update -p ComputerSales.Infrastructure -s API_ComputerProject


//Không overwrite. Hãy tạo migration mới mô tả thay đổi tiếp theo (thêm bảng/đổi cột…):
/*
 * 
 * (1)  dùng để migragtion qua sql thành 1 table nhưng không cần xóa đến các migration trước 
 * 
dotnet ef migrations add TenMigrationMoiUpdateVedieuGiDo -p ComputerSales.Infrastructure -s API_ComputerProject

// (2) sau khi thực hiện (1) ta thực hiện lại cập nhật lại migration bằng cầu lệnh 

dotnet ef database update -p ComputerSales.Infrastructure -s API_ComputerProject


// luôn luôn phải fetch và pull trước khi run dự án git clone về
-> git changes -> view all commmits -> incomming -> fetch -> full

 */


