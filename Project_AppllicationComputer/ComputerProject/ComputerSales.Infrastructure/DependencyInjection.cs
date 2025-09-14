using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCase.Order_UC;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCase.Role_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.Validator.AccountValidator;
using ComputerSales.Application.Validator.CustomerValidator;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Infrastructure.Repositories.Account_Respo;
using ComputerSales.Infrastructure.Repositories.Product_Respo;
using ComputerSales.Infrastructure.Repositories.ProductOptionalType_Respository;
using ComputerSales.Infrastructure.Repositories.Respository_ImplementationInterface;
using ComputerSales.Infrastructure.Repositories.Role_Respo;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ComputerSales.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(
               config.GetConnectionString("Quy"),
               sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));
            services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();
            services.AddScoped<IProductRespository, ProductRespository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountRepository, AccountRespository>();
            services.AddScoped<IValidator<AccountDTOInput>, CreateAccountValidator>();
            services.AddScoped<IValidator<CustomerInputDTO>, CreateCustomerValidator>();
            services.AddScoped<IProductOptionalTypeRespositorycs, ProdcutOptionalType_Respository>();
          
            services.AddScoped(typeof(IRespository<>), typeof(EfRepository<>)); //Depedency Injection cho các class sử dụng 
            return services;
        }

        public static IServiceCollection AddApplicationUseCase(this IServiceCollection services)
        {
            /********************Product**************************/
           services.AddScoped<CreateProduct_UC>();
           services.AddScoped<GetProduct_UC>();
           services.AddScoped<UpdateProduct_UC>();
           services.AddScoped<DeleteProduct_UC>();
            /*****************************************************/

            //==============    Role    ================//
           services.AddScoped<CreateRole_UC>();
           services.AddScoped<GetRole_UC>();
           services.AddScoped<UpdateRole_UC>();
           services.AddScoped<DeleteRole_UC>();

            //==============    Accounts    ===============//
           services.AddScoped<CreateAccount_UC>();
           services.AddScoped<UpdateAccount_UC>();
           services.AddScoped<GetAccount_UC>();
            services.AddScoped<GetAccountByEmail_UC>();
           services.AddScoped<DeleteAccount_UC>();

            /********************Product Over View**************************/
           services.AddScoped<CreateProductOverView_UC>();
           services.AddScoped<DeleteProductOverView_UC>();
           services.AddScoped<GetByIdProductOverView_UC>();
           services.AddScoped<UpdateProductOverView_UC>();
            /*****************************************************/

            /********************Product Protection**************************/
           services.AddScoped<CreateProductProtection_UC>();
           services.AddScoped<DeleteProductProtection_UC>();
           services.AddScoped<GetByIdProductProtection_UC>();
           services.AddScoped<UpdateProductProtection_UC>();
            /*****************************************************/

            //============  Order   ================//
           services.AddScoped<CreateOrder_UC>();
            //==========================================


            //================= Account ==============//
           services.AddScoped<CreateCustomer_UC>();
           services.AddScoped<DeleteCustomer_UC>();
           services.AddScoped<getCustomerByID>();

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


