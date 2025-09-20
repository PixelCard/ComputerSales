using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Domain.Entity;
using ComputerSales.Domain.Entity.ECustomer;
using ComputerSales.Infrastructure.Sercurity.JWT.Interface;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ComputerSalesProject_MVC.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        //---------------------------------------Attribute--------------------------------------------------
        private readonly IJwtTokenGenerator _jwt;
        private readonly IAccountRepository _accountService;
        private readonly IRoleRepository _roleService;
        private readonly IUnitOfWorkApplication _uow;
        private readonly IResfreshTokenRespo _refresh;

        //---------------------------------------Constructor--------------------------------------------------
        public AccountController(
            IJwtTokenGenerator jwt, 
            IAccountRepository accountService, 
            IRoleRepository roleService, 
            IUnitOfWorkApplication uow,
            IResfreshTokenRespo refresh)
        {
            _jwt = jwt;
            _accountService = accountService;
            _roleService = roleService;
            _uow = uow;
            _refresh = refresh;
        }

        //---------------------------------------Get--------------------------------------------------
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register() => View(); 


        //---------------------------------------Post--------------------------------------------------
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm, CancellationToken ct)
        {
            var acc = await _accountService.GetAccountByEmail(vm.email, ct);

            //var hash = BCrypt.Net.BCrypt.HashPassword(vm.pass); 
            
            //Không nên hash pass rồi so sánh vs pass ma lấy ra từ account 

            //Bởi vì nó sẽ làm ra các trường id khác nhau làm cho dù nó có cùng mã hash nhưng khác salt(id  hash) khác nên sẽ khác

            if (acc == null || !BCrypt.Net.BCrypt.Verify(vm.pass, acc.Pass))
            {
                ModelState.AddModelError("", "Sai tài khoản/mật khẩu");
                return View(vm);
            }

            if (acc.Role == null) acc.Role = await _roleService.GetRole(acc.IDRole, ct);

            var token = _jwt.Generate(acc);


            // Lưu JWT vào cookie (HTTP-only)
            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });


            // Refresh token (dài hạn) -> lưu DB 
            var rt = await _refresh.IssueAsync(acc, ct);

            return RedirectToAction("Index", "Home");
        }


        [ValidateAntiForgeryToken]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel req, CancellationToken ct)
        {
            // 1) Validate cơ bản
            var email = (req.Email ?? "").Trim().ToLowerInvariant();
            
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Email và mật khẩu là bắt buộc." });

            // 2) Check trùng email
            var existed = await _accountService.GetAccountByEmail(email, ct); 

            if (existed != null)
                return Conflict(new { message = "Email đã tồn tại." });

            // 3) Xác định RoleId
            var roleId = req.RoleId ?? 1;

            if (!ModelState.IsValid) return View(req);

            // 4) Hash mật khẩu 
            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password); // luôn hash ở server


            // 4.1) Tạo Account + gán Customer (1-1)
            var acc = Account.Create(email, hash, roleId);

            // Nếu bạn đã có entity Customer, map các field tương ứng ở đây:
            acc.Customer = new Customer
            {
                Name = req.UserName,                  
                Description = req.Description_User,       
                Date = req.Date.Value
            };

            await _uow.BeginTransactionAsync(ct);

            try
            {

                // 5) Lưu DB
                await _accountService.AddAccount(acc, ct);
                await _uow.SaveChangesAsync(ct);

                // 6) Nạp Role để nhúng claim (nếu navigation chưa được load)
                acc.Role = acc.Role ?? await _roleService.GetRole(acc.IDRole, ct);

                // 7) Sinh JWT
                var token = _jwt.Generate(acc);

                await _uow.CommitAsync(ct);

                // 8) Trả về Trang home
                return RedirectToAction("Login", "Account");

            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(ct);
                throw;
            }
        }


        // ====== Refresh (MVC) ======
        // Gọi khi access token hết hạn (ví dụ Ajax POST tới /Account/Refresh)
        [HttpPost("Refresh")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refresh(string? returnUrl, CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Missing refresh token");

            var active = await _refresh.GetActiveAsync(refreshToken, ct);
            if (active == null)
                return Unauthorized("Invalid/expired refresh token");

            // phát access token mới từ account của refresh token
            var newAccess = _jwt.Generate(active.Account);

            Response.Cookies.Append("access_token", newAccess, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });


            // --- chọn nơi để quay lại ---
            string? target = null;


            // 1) ưu tiên returnUrl nếu hợp lệ (local)
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                target = returnUrl;


            // 2) fallback: dùng Referer (trang trước đó)
            if (target == null)
            {
                var referer = Request.Headers["Referer"].ToString();
                if (Uri.TryCreate(referer, UriKind.Absolute, out var uri))
                {
                    var path = uri.PathAndQuery;
                    if (Url.IsLocalUrl(path) && !path.StartsWith("/Account", StringComparison.OrdinalIgnoreCase))
                        target = path;
                }
            }

            // 3) cuối cùng: về Home
            return target != null
                ? LocalRedirect(target)
                : RedirectToAction("Index", "Home");
        }

        // ====== Logout (MVC) ======
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (!string.IsNullOrEmpty(refreshToken))
                await _refresh.RevokeAsync(refreshToken, ct);

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return RedirectToAction(nameof(Login));
        }
    }
}





