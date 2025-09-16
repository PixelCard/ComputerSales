using ComputerSales.Domain.Entity;

namespace ComputerSales.Infrastructure.Sercurity.JWT.Interface
{
    public interface IJwtTokenGenerator
    {
        string Generate(Account account);
    }
}
