using ComputerSales.Application.Common.Mapping;

namespace API_ComputerProject.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

    }
}
