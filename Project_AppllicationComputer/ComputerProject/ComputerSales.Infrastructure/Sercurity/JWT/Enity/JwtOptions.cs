namespace ComputerSales.Infrastructure.Sercurity.JWT.Enity
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string Key { get; set; } = default!;   
        public int ExpireMinutes { get; set; } = 60;
    }
}
