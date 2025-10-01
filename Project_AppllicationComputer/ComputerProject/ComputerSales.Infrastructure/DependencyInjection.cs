using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Cart_Interface;
using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Application.Interface.Interface_OrderFromCart;
using ComputerSales.Application.Interface.InterFace_ProductOptionalType_Respository;
using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Interface.Interface_VariantPriceRespo;
using ComputerSales.Application.Interface.InterfaceCustomerRespo;
using ComputerSales.Application.Interface.InterfaceRespository;
using ComputerSales.Application.Interface.Product_Interface;
using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Cart_UC.Commands.AddCart;
using ComputerSales.Application.UseCase.Cart_UC.Commands.RemoveItem;
using ComputerSales.Application.UseCase.Cart_UC.Commands.UpdateQuantity;
using ComputerSales.Application.UseCase.Cart_UC.Queries.GetCartPage;
using ComputerSales.Application.UseCase.Category_UC;
using ComputerSales.Application.UseCase.Customer_UC;
using ComputerSales.Application.UseCase.OptionalType_UC;
using ComputerSales.Application.UseCase.OptionalValue_UC;
using ComputerSales.Application.UseCase.Order_UC;
using ComputerSales.Application.UseCase.Product_UC;
using ComputerSales.Application.UseCase.ProductOvetView_UC;
using ComputerSales.Application.UseCase.ProductProtection_UC;
using ComputerSales.Application.UseCase.ProductVariant_UC;
using ComputerSales.Application.UseCase.Role_UC;
using ComputerSales.Application.UseCase.VariantImage_UC;
using ComputerSales.Application.UseCase.VariantPrice_UC.variantGetPriceByVariantID;
using ComputerSales.Application.UseCaseDTO.Account_DTO;
using ComputerSales.Application.UseCaseDTO.Customer_DTO;
using ComputerSales.Application.Validator.AccountValidator;
using ComputerSales.Application.Validator.CustomerValidator;
using ComputerSales.Infrastructure.Persistence;
using ComputerSales.Infrastructure.Repositories;
using ComputerSales.Infrastructure.Repositories.Account_Respo;
using ComputerSales.Infrastructure.Repositories.Cart_Respo.CartRead;
using ComputerSales.Infrastructure.Repositories.Cart_Respo.CartWrite;
using ComputerSales.Infrastructure.Repositories.Customer_Respo;
using ComputerSales.Infrastructure.Repositories.EmailVerifyKeyRepository;
using ComputerSales.Infrastructure.Repositories.OrderCart_Respo;
using ComputerSales.Infrastructure.Repositories.Product_Respo;
using ComputerSales.Infrastructure.Repositories.ProductOptionalType_Respository;
using ComputerSales.Infrastructure.Repositories.RefreshToken_Respo;
using ComputerSales.Infrastructure.Repositories.Respository_ImplementationInterface;
using ComputerSales.Infrastructure.Repositories.Role_Respo;
using ComputerSales.Infrastructure.Repositories.SmtpEmailSender_Respository;
using ComputerSales.Infrastructure.Repositories.UnitOfWork;
using ComputerSales.Infrastructure.Repositories.VariantPrice_Respo;
using ComputerSales.Application.Sercurity.JWT.Enity;
using ComputerSales.Application.Sercurity.JWT.Interface;
using ComputerSales.Application.Sercurity.JWT.Respository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ComputerSales.Infrastructure.Repositories.ForgetPassRespo;
using ComputerSales.Application.Interface.Interface_ForgetPassword;
using ComputerSales.Application.UseCase.ForgetPass_UC;
using ComputerSales.Application.Payment.Interface;
using ComputerSales.Application.Payment.VNPAY.Respository;
using ComputerSales.Application.Interface.InterfaceVNPAYMENT;
using ComputerSales.Infrastructure.Repositories.VNPAYMENTRespo;
using ComputerSales.Application.UseCase.VariantPrice_UC;
using ComputerSales.Application.UseCase.VariantOptionValue_UC;
using ComputerSales.Application.UseCase.ProductOptionalType_UC;


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

            services.AddMemoryCache();

            services.AddScoped<IUnitOfWorkApplication, UnitOfWork_Infa>();
            services.AddScoped<IProductRespository, ProductRespository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAccountRepository, AccountRespository>();
            services.AddScoped<IValidator<AccountDTOInput>, CreateAccountValidator>();
            services.AddScoped<IValidator<CustomerInputDTO>, CreateCustomerValidator>();
            services.AddScoped<IProductOptionalTypeRespositorycs, ProdcutOptionalType_Respository>();
            services.AddScoped<ICartReadRespository, CartReadRespository>();
            services.AddScoped<IOrderFromCart, OrderFromCartRespository>();
            services.AddScoped<ICartWriteRepository, CartWriteRepository>();
            services.AddScoped<IVariantPriceRespo, VariantPriceRespo>();
            services.AddScoped<ICustomerRespo, CustomerRespo>();
            services.AddScoped<IEmailVerifyKeyRepository, EmailVerifyKeyRepository>();
            services.AddScoped<IEmailSender, SmtpEmailSenderRespo>();
            services.AddScoped(typeof(IRespository<>), typeof(EfRepository<>)); //Depedency Injection cho các class sử dụng 
            services.AddScoped<ProviderRepository>();

            //ForgetPass
            services.AddScoped<IForgotPasswordRespo, ForgotPasswordStoreMemoryRespo>();


            // JWT generator
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.Configure<JwtOptions>(config.GetSection("Jwt"));
            services.AddScoped<IResfreshTokenRespo, RefreshTokenRespo>();


            //VNPAY
            services.AddScoped<IVnPayService, VnPayService>();

            services.AddScoped<IVnPaySessionService, VnPaySessionServiceRespo>();




            return services;
        }

        public static IServiceCollection AddApplicationUseCase(this IServiceCollection services)
        {

            //=======================================================================================================================

            /********************Product**************************/
           services.AddScoped<CreateProduct_UC>();
           services.AddScoped<GetProduct_UC>();
           services.AddScoped<UpdateProduct_UC>();
           services.AddScoped<DeleteProduct_UC>();
            /*****************************************************/

            /********************Product OverView **************************/
            services.AddScoped<CreateProductOverView_UC>();
            services.AddScoped<DeleteProductOverView_UC>();
            services.AddScoped<GetByIdProductOverView_UC>();
            services.AddScoped<UpdateProductOverView_UC>();
            /*****************************************************/

            //================= Product Varient    ===============//
            services.AddScoped<CreateProductVariant_UC>();
            services.AddScoped<DeleteProductVariant_UC>();
            services.AddScoped<GetByIdProductVariant_UC>();
            services.AddScoped<UpdateProductVariant_UC>();

            //==================    VariantImage   ==================//
            services.AddScoped<CreateVariantImage_UC>();
            services.AddScoped<DeleteVariantImage_UC>();
            services.AddScoped<getVariantImageById_UC>();
            services.AddScoped<UpdateVariantImage_UC>();

            //================ Provider ==================//
            services.AddScoped<CreateCategory_UC>();
            services.AddScoped<DeleteCategory_UC>();
            services.AddScoped<UpdateCategory_UC>();
            services.AddScoped<GetByIdCategory_UC>();

            //========================      OptionType      =====================//
            services.AddScoped<CreateOptionalType_UC>();
            services.AddScoped<DeleteOptionalType_UC>();
            services.AddScoped<GetByIdOptionalType_UC>();
            services.AddScoped<UpdateOptionalType_UC>();

            //========================      OptionValue      =====================//
            services.AddScoped<CreateOptionalValue_UC>();
            services.AddScoped<DeleteOptionalValue_UC>();
            services.AddScoped<GetByIdOptionalValue_UC>();
            services.AddScoped<UpdateOptionalValue_UC>();


            //

            //=====================================================================================================




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

        

            /********************Product Protection**************************/
           services.AddScoped<CreateProductProtection_UC>();
           services.AddScoped<DeleteProductProtection_UC>();
           services.AddScoped<GetByIdProductProtection_UC>();
           services.AddScoped<UpdateProductProtection_UC>();
            /*****************************************************/

            //============  Order   ================//
            services.AddScoped<CreateOrder_UC>();
            services.AddScoped<DeleteOrder_UC>();
            services.AddScoped<GetOrderByIDCustomerAndOrderID>();
            services.AddScoped<UpdateOrder_UC>();
            //==========================================


            //================= Account ==============//
            services.AddScoped<CreateCustomer_UC>();
            services.AddScoped<DeleteCustomer_UC>();
            services.AddScoped<getCustomerByID>();
            services.AddScoped<RegisterAccount_UC>();
            services.AddScoped<VerifyEmail_UC>();
            services.AddScoped<ResendVerifyEmail_UC>();


            //================= Cart ==============//
            services.AddScoped<GetCartPageQueryHandler>();
            services.AddScoped<UpdateQuantityCommandHandler>();
            services.AddScoped<RemoveItemCommandHandler>();
            services.AddScoped<AddItemCommandHandler>();


            //================= Customer ==============//
            services.AddScoped<getCustomerByUserID>();

            services.AddScoped<getCustomerByID>();

            services.AddScoped<DeleteCustomer_UC>();

            services.AddScoped<CreateCustomer_UC>();

            services.AddScoped<UpdateCustomer_UC>();

            //================= Variant Price ==============//
            services.AddScoped<variantGetPriceByVariantID_UC>();

            services.AddScoped<CreateVariantPrice_UC>();
            services.AddScoped<UpdateVariantPrice_UC>();
            services.AddScoped<DeleteVariantPrice_UC>();
            services.AddScoped<GetByIdVariantPrice_UC>();


            //================= Variant Option Value =================//
            services.AddScoped<CreateVariantOptionValue_UC>();
            services.AddScoped<UpdateVariantOptionValue_UC>();
            services.AddScoped<GetByIdVariantOptionValue_UC>();
            services.AddScoped<DeleteVariantOptionValue_UC>();

            //============== ProductOptionType  ==============
            services.AddScoped<CreateProductOptionalType_UC>();
            services.AddScoped<DeleteProductOptionalType_UC>();
            services.AddScoped<GetByIdProductOptionalType_UC>();
            services.AddScoped<UpdateProductOptionalType_UC>();


            //================= Forget Pass ==============//
            services.AddScoped<ForgotResetPassword_UC>();
            services.AddScoped<ForgotVerifyOtp_UC>();
            services.AddScoped<ForgotRequestOtp_UC>();

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


