using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCase.Customer_UC;

namespace ComputerSalesProject_MVC.DependencyInjetionServices
{
    public static class DependencyInjection_UseCaseMVC
    {
        public static IServiceCollection AddUseCaseMVC(this IServiceCollection services)
        {
            //------------------------Account-------------------------------
            services.AddScoped<CreateAccount_UC>();


            //------------------------Customer-------------------------------
            services.AddScoped<CreateCustomer_UC>();

            return services;
        }
    }
}
