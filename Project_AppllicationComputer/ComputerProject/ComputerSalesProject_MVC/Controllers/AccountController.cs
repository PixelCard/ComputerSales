using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_RefreshTokenRespository;
using ComputerSales.Application.Interface.Role_Interface;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Application.UseCase.Account_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.EmailVerify_DTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.RegisterDTO;
using ComputerSales.Application.UseCaseDTO.Account_DTO.ResendVerifyEmaiDTO;
using ComputerSales.Application.Sercurity.JWT.Interface;
using ComputerSalesProject_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComputerSales.Application.Interface.Interface_Email_Respository;
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
        private readonly IEmailSender emailSender;
        private readonly IResfreshTokenRespo _refresh;
        private readonly IConfiguration _cfg;
        private readonly RegisterAccount_UC _register;
        private readonly VerifyEmail_UC _verify;
        private readonly ResendVerifyEmail_UC _resend;

        public AccountController(IJwtTokenGenerator jwt, 
            IAccountRepository accountService, 
            IRoleRepository roleService, 
            IUnitOfWorkApplication uow, 
            IResfreshTokenRespo refresh, 
            RegisterAccount_UC register, VerifyEmail_UC verify, 
            ResendVerifyEmail_UC resend,
            IConfiguration _cfg,
            IEmailSender emailSender)
        {
            _jwt = jwt;
            _accountService = accountService;
            _roleService = roleService;
            _uow = uow;
            _refresh = refresh;
            _register = register;
            _verify = verify;
            _resend = resend;
            this._cfg = _cfg;
            this.emailSender = emailSender;
        }

        //---------------------------------------Constructor--------------------------------------------------


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
            if (ModelState.IsValid)
            {
                var acc = await _accountService.GetAccountByEmail(vm.email, ct);


                if (acc == null || !BCrypt.Net.BCrypt.Verify(vm.pass, acc.Pass))
                {
                    ModelState.AddModelError("", "Sai tài khoản/mật khẩu");
                    return View(vm);
                }


                if (acc.Role == null) acc.Role = await _roleService.GetRole(acc.IDRole, ct);


                // Kiểm tra xem tài khoản đã xác thực email chưa
                // Chưa xác thực email
                if (!acc.EmailConfirmed)
                {
                    // Hết hạn 15 ngày
                    if (acc.CreatedAt.AddDays(15) < DateTime.UtcNow)
                    {
                        await _accountService.DeleteAccountAsync(acc.IDAccount, ct);
                        ModelState.AddModelError("", "Tài khoản đã hết hạn vì chưa xác thực email. Vui lòng đăng ký lại.");
                        return View(vm);
                    }

                    // GỬI LẠI EMAIL Ở ĐÂY 
                    await _resend.Handle(new ResendVerifyEmailDTO(acc.IDAccount), ct);

                    // Lấy hạn mới (nếu UC có cập nhật)
                    var fresh = await _accountService.GetAccountByID(acc.IDAccount, ct);
                    TempData["Info"] = "Tài khoản chưa xác thực. Chúng tôi vừa gửi lại email xác thực.";
                    TempData["ExpUtc"] = fresh?.VerifyKeyExpiresAt?.ToString("o");

                    // Điều hướng sang trang thông báo
                    return RedirectToAction("PendingVerify", new { uid = acc.IDAccount });
                }


                var token = _jwt.Generate(acc);


                // Lưu JWT vào cookie (HTTP-only)
                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });


                // Refresh token (dài hạn) -> lưu DB 
                var rt = await _refresh.IssueAsync(acc, ct);


                // Lưu Resfresh Token vào cookie (HTTP-only)
                Response.Cookies.Append("refresh_token", rt.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(15)
                });


                var Role = await _roleService.GetRole(acc.IDRole, ct);


                if (Role.TenRole == "admin")
                {
                    return RedirectToAction("AdminLayout", "AdminHome", new { Area = "Admin" });
                }



                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }



        [ValidateAntiForgeryToken]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequestDTO req, CancellationToken ct)
        {
            // Chuẩn hóa email
            var email = (req.Email ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
            {
                ModelState.AddModelError(string.Empty, "Email và mật khẩu là bắt buộc.");
                return View(req);
            }

            // Kiểm tra trùng email
            var existed = await _accountService.GetAccountByEmail(email, ct);
            if (existed != null)
            {
                ModelState.AddModelError(string.Empty, "Email đã tồn tại.");
                return View(req);
            }

            var birthDate = req.Date;  // Giả sử `req.Date` chứa ngày sinh người dùng

            if (birthDate == null || birthDate > DateTime.Now.AddYears(-16))
            {
                ModelState.AddModelError(string.Empty, "Tuổi phải lớn hơn hoặc bằng 16.");
                return View(req);
            }



            try
            {
                // UC sẽ: tạo Account, tạo verify key 60s, set VerifyKeyExpiresAt, gửi email, commit
                await _register.Handle(req, ct);

                // LẤY LẠI account theo email để biết ID và thời điểm hết hạn verify.
                var acc = await _accountService.GetAccountByEmail(email, ct);

                if (acc == null)
                {
                    // Phòng hờ: nếu vì lý do gì đó chưa đọc lại được account
                    TempData["Info"] = "Đăng ký thành công. Vui lòng kiểm tra email để xác thực.";
                    return RedirectToAction(nameof(Login));
                }

                // 2) Lấy mốc hết hạn đếm ngược cho màn PendingVerify.
                //    UC khi tạo key cần set acc.VerifyKeyExpiresAt = expireAt;
                //    nếu vì lý do nào đó chưa có, fallback +60s để UI vẫn hiển thị.
                var expUtc = acc.VerifyKeyExpiresAt ?? DateTime.UtcNow.AddSeconds(60);

                // 3) Điều hướng sang trang “PendingVerify”,
                //  truyền kèm:
                //    - uid: để link “Gửi lại” (Resend) biết account nào
                //    - expUtc: để View hiển thị đồng hồ đếm ngược chính xác
                return RedirectToAction(nameof(PendingVerify), new
                {
                    uid = acc.IDAccount,
                    expUtc = expUtc.ToString("o")
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(req);
            }
        }



        // =================== PENDING VERIFY (đếm ngược) ===================
        [HttpGet("PendingVerify")]
        [AllowAnonymous]
        public IActionResult PendingVerify([FromQuery] int uid, [FromQuery] string expUtc)
        {
            DateTime expiresAtUtc =
                DateTime.TryParse(expUtc, null, System.Globalization.DateTimeStyles.RoundtripKind, out var t)
                ? t
                : DateTime.UtcNow.AddSeconds(60);

            var vm = new PendingVerifyRequest
            {
                AccountId = uid,
                VerifyExpiresAtUtc = expiresAtUtc
            };

            return View(vm); // Views/Account/PendingVerify.cshtml
        }



        // =================== VERIFY (link trong email) ===================
        // Ví dụ link: /Account/Verify?uid=123&key=Base64UrlRawKey
        [HttpGet("Verify")]
        [AllowAnonymous]
        public async Task<IActionResult> Verify([FromQuery] int uid, [FromQuery] string key, CancellationToken ct)
        { 
            if (uid <= 0 || string.IsNullOrWhiteSpace(key))
                return BadRequest("Thiếu tham số xác thực.");

            try
            {
                await _verify.Handle(new VerifyEmailRequest(uid, key),ct);
                TempData["SuccessMessage"] = "Xác thực email thành công!";
                return RedirectToAction(nameof(VerifySuccess));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                // lấy lại expUtc (nếu còn) để tiếp tục hiển thị countdown
                var acc = await _accountService.GetAccountByID(uid, ct);
                var expUtc = acc?.VerifyKeyExpiresAt ?? DateTime.UtcNow.AddSeconds(60);

                return RedirectToAction(nameof(PendingVerify), new
                {
                    uid,
                    expUtc = expUtc.ToString("o")
                });
            }
        }



        [HttpGet("VerifySuccess")]
        [AllowAnonymous]
        public IActionResult VerifySuccess() => View(); // Views/Account/VerifySuccess.cshtml



        // =================== RESEND VERIFY (nút trên PendingVerify) ===================
        [HttpPost("ResendVerify")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendVerify([FromForm] int uid, CancellationToken ct)
        {
            if (uid <= 0)
                return BadRequest(new { ok = false, message = "Thiếu uid." });

            try
            {
                // Tạo đối tượng ResendVerifyEmailDTO với AccountId
                var request = new ResendVerifyEmailDTO(uid);

                // Gọi use case ResendVerifyEmail_UC để gửi lại email xác thực
                await _resend.Handle(request, ct);

                // Đọc lại account để lấy VerifyKeyExpiresAt vừa set trong UC
                var acc = await _accountService.GetAccountByID(uid, ct);
                var newExpireAtUtc = acc?.VerifyKeyExpiresAt ?? DateTime.UtcNow.AddSeconds(60);

                // Trả về thông tin xác thực mới
                return Ok(new
                {
                    ok = true,
                    expUtc = newExpireAtUtc.ToString("o")
                });
            }
            catch (Exception ex)
            {
                // Trả về thông báo lỗi nếu có lỗi xảy ra
                return BadRequest(new { ok = false, message = ex.Message });
            }
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





