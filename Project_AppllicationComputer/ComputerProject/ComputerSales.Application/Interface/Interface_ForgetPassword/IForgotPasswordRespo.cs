using ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.Interface.Interface_ForgetPassword
{
    public interface IForgotPasswordRespo
    {
        // OTP
        Task SetOtpAsync(string email, string code, TimeSpan ttl, CancellationToken ct);
        Task InvalidateOtpAsync(string email, CancellationToken ct);
        Task<OtpVerifyResult> VerifyOtpAsync(string email, string code, CancellationToken ct);

        // Reset session token (opaque, server-side)
        Task<string> IssueResetSessionAsync(string email, TimeSpan ttl, CancellationToken ct);
        Task<bool> ValidateAndConsumeResetSessionAsync(string email, string token, CancellationToken ct);
    }
}

