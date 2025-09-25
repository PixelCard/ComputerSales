using ComputerSales.Application.UseCase.ForgetPass_UC;
using ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerSalesProject_MVC.Controllers
{

    [Route("[controller]")]
    [AllowAnonymous]
    public class ForgetPasswordController : Controller
    {
        private readonly ForgotRequestOtp_UC _fpRequest;
        private readonly ForgotVerifyOtp_UC _fpVerify;
        private readonly ForgotResetPassword_UC _fpReset;
        public ForgetPasswordController(ForgotRequestOtp_UC fpRequest, ForgotVerifyOtp_UC fpVerify, ForgotResetPassword_UC fpReset)
        {
            _fpRequest = fpRequest;
            _fpVerify = fpVerify;
            _fpReset = fpReset;
        }

        // ===== View =====
        [HttpGet("ForgetPassHome")]
        public IActionResult ForgetPassHome() => View();

        [HttpPost("RequestOtp")]
        public async Task<IActionResult> RequestOtp([FromBody] ForgetResquestDTO dto, CancellationToken ct)
        {
            var email = (dto?.Email ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { success = false, message = "Email is required." });

            await _fpRequest.HandleAsync(email, ct);
            return Ok(new { success = true });
        }


        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] ForgotVerifyDto dto, CancellationToken ct)
        {
            var email = (dto?.Email ?? "").Trim().ToLowerInvariant();
            var code = (dto?.Code ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
                return BadRequest(new { success = false, message = "Email and code are required." });

            if (!System.Text.RegularExpressions.Regex.IsMatch(code, @"^\d{4}$"))
                return BadRequest(new { success = false, message = "Code must be 4 digits." });

            var resetToken = await _fpVerify.HandleAsync(email, code, ct);

            if (resetToken == null)
                return BadRequest(new { success = false, message = "Invalid or expired code." });

            return Ok(new { success = true, resetToken });
        }


        // ===== API: đặt lại mật khẩu (dùng resetToken) =====
        [HttpPost("Reset")]
        public async Task<IActionResult> Reset([FromBody] ForgotResetDto dto, CancellationToken ct)
        {
            var email = (dto?.Email ?? "").Trim().ToLowerInvariant();
            var token = (dto?.Token ?? "").Trim();
            var pw = (dto?.NewPassword ?? "").Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(pw))
                return BadRequest(new { success = false, message = "Invalid payload." });

            if (pw.Length < 8)
                return BadRequest(new { success = false, message = "Password must be at least 8 characters long." });

            var ok = await _fpReset.HandleAsync(email, token, pw, ct);
            return ok ? Ok(new { success = true })
                      : BadRequest(new { success = false, message = "Reset failed." });
        }
    }
}
