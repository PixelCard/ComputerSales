using ComputerSales.Application.Interface.Interface_ForgetPassword;
using ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.ForgetPass_UC
{
    public class ForgotVerifyOtp_UC
    {
        private readonly IForgotPasswordRespo _store;


        public ForgotVerifyOtp_UC(IForgotPasswordRespo store)
        {
            _store = store;
        }


        public async Task<string?> HandleAsync(string email, string code, CancellationToken ct)
        {
            var result = await _store.VerifyOtpAsync(email, code, ct);
            if (result == OtpVerifyResult.Ok)
            {
                var resetToken = await _store.IssueResetSessionAsync(email, TimeSpan.FromMinutes(10), ct);
                return resetToken;
            }
            // xử lý Mismatch/Expired/TooManyAttempts/NotFound...
            return null;
        }
    }
}
