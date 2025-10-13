using ComputerSales.Application.Sercurity.JWT.Enity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ComputerSales.Application.Sercurity.JWT.Extensions
{
    public static class JwtServiceCollectionExtensions
    {

        //Cấu hình cho ACCESS token
        public static IServiceCollection AddJwtAuth(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName = "Jwt",
        bool requireHttps = false,
        Action<JwtBearerOptions>? extra = null) {

            services.Configure<JwtOptions>(config.GetSection(sectionName));


            var jwt = config.GetSection(sectionName).Get<JwtOptions>()
                      ?? throw new InvalidOperationException($"Missing '{sectionName}' config.");


            if (string.IsNullOrWhiteSpace(jwt.Key))
                throw new InvalidOperationException("Jwt:Key is required.");

            var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);

            services
                .AddAuthentication(o =>
                {
                    //dùng JWT để xác thực & challenge mặc định
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = requireHttps; //khi true middleware sẽ yêu cầu metadata qua HTTPS(bật ở prod)
                    o.SaveToken = true; // lưu token đã validated vào HttpContext

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,             // kiểm tra Issuer khớp
                        ValidateAudience = true,           // kiểm tra Audience khớp
                        ValidateLifetime = true,           // kiểm tra hạn token + ClockSkew
                        ValidateIssuerSigningKey = true,   // kiểm tra chữ ký (HMAC)
                        ValidIssuer = jwt.Issuer,
                        ValidAudience = jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ClockSkew = TimeSpan.Zero          // bỏ lệch mặc định 5 phút
                    };

                    //Nhận 1 key từ Cookie sau đó gắn vào cho JWT
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (string.IsNullOrEmpty(ctx.Token) &&
                                ctx.Request.Cookies.TryGetValue("access_token", out var t))
                            {
                                ctx.Token = t; // Lấy token từ cookie khi không có Authorization header
                            }
                            return Task.CompletedTask;
                        }
                    };

                    // cho phép caller bổ sung cấu hình handler (events, map inbound claims…)
                    extra?.Invoke(o);
                });

            services.AddAuthorization(); // có thể AddPolicy ở đây nếu muốn

            return services;
        }
    }
}
