using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Sercurity.JWT.Enity;
using ComputerSales.Application.Sercurity.JWT.Interface;
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
            var path = ctx.Request.Path.Value ?? "";  //Lấy ra URL đang truy cập

            //chỉ chạy khi chưa đăng nhập
            if (!ctx.User.Identity?.IsAuthenticated ?? true)
                if (ctx.Request.Method == "GET" &&
                    !path.StartsWith("/Account", StringComparison.OrdinalIgnoreCase))
                {
                    //Đọc refresh token từ cookie:
                    if (ctx.Request.Cookies.TryGetValue("refresh_token", out var rt))
                    {
                        //gọi repo kiểm tra còn active không:
                        var active = await refreshRepo.GetActiveAsync(rt);

                        //Hợp lệ (chưa hết hạn, chưa bị revoke)
                        if (active != null)
                        {
                            //sinh access token mới
                            var token = jwt.Generate(active.Account);

                            // dùng cho request hiện tại
                            ctx.Request.Headers["Authorization"] = "Bearer " + token;


                            ctx.Response.Cookies.Append("access_token", token, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = false, // PROD: true
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddMinutes(opt.Value.ExpireMinutes)
                            });
                        }
                    }
                }
            //Chuyển tiếp pipeline cho middleware tiếp theo.
            await _next(ctx);
        }
    }
}


/*
 User vào trang GET nào đó → nếu chưa đăng nhập nhưng còn refresh_token hợp lệ 
→ middleware cấp access token mới (set vào cookie; tuỳ chọn chèn vào header để dùng ngay) → tiếp tục request.

Lần truy cập kế tiếp (hoặc ngay lập tức nếu bạn chèn header) sẽ qua [Authorize] bình thường.
 */