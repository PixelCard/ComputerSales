using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Sercurity.JWT.Enity;
using ComputerSales.Application.Sercurity.JWT.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ComputerSalesProject_MVC.MiddleWareCustome
{
    public class AutoRefreshAccessMiddleware
    {
        private readonly RequestDelegate _next;
        public AutoRefreshAccessMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx,
            IResfreshTokenRespo refreshRepo,
            IJwtTokenGenerator jwt,
            IOptions<JwtOptions> opt)
        {
            var path = ctx.Request.Path.Value ?? "";

            // Chỉ GET, không phải khu vực account/auth (tránh vòng lặp), và chưa authenticated
            if (ctx.Request.Method == HttpMethods.Get &&
                !path.StartsWith("/Account", StringComparison.OrdinalIgnoreCase) &&
                !path.StartsWith("/auth", StringComparison.OrdinalIgnoreCase) &&
                !(ctx.User?.Identity?.IsAuthenticated ?? false))
            {
                // Đọc refresh_token (Path="/" như trên)
                if (ctx.Request.Cookies.TryGetValue("refresh_token", out var rt))
                {
                    var active = await refreshRepo.GetActiveAsync(rt); // verify + chưa revoke + chưa hết hạn
                    if (active != null)
                    {

                        // Cấp access token mới
                        var access = jwt.Generate(active.Account);

                        // Cho request hiện tại: chèn Authorization header
                        ctx.Request.Headers["Authorization"] = "Bearer " + access;

                        // Ghi cookie access mới
                        ctx.Response.Cookies.Append("access_token", access, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Lax,   // hoặc Strict
                            Path = "/",                // dùng toàn site
                            Expires = DateTimeOffset.UtcNow.AddMinutes(opt.Value.ExpireMinutes)
                        });

                        // Re-auth ngay lập tức để request hiện tại qua được [Authorize]
                        var authResult = await ctx.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                        if (authResult.Succeeded && authResult.Principal != null)
                            ctx.User = authResult.Principal;
                    }
                }
            }
            await _next(ctx);
        }
    }
}


/*
 User vào trang GET nào đó → nếu chưa đăng nhập nhưng còn refresh_token hợp lệ 
→ middleware cấp access token mới (set vào cookie; tuỳ chọn chèn vào header để dùng ngay) → tiếp tục request.

Lần truy cập kế tiếp (hoặc ngay lập tức nếu bạn chèn header) sẽ qua [Authorize] bình thường.
 */