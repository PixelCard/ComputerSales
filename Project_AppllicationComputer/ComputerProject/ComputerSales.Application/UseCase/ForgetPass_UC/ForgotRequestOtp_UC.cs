using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_Email_Respository;
using ComputerSales.Application.Interface.Interface_ForgetPassword;

namespace ComputerSales.Application.UseCase.ForgetPass_UC
{
    // Request OTP
    public class ForgotRequestOtp_UC
    {
        private readonly IAccountRepository _accounts;
        private readonly IForgotPasswordRespo _store;
        private readonly IEmailSender _mail;
        public ForgotRequestOtp_UC(IAccountRepository a, IForgotPasswordRespo s, IEmailSender m)
        { _accounts = a; _store = s; _mail = m; }

        public async Task HandleAsync(string email, CancellationToken ct)
        {
            var acc = await _accounts.GetAccountByEmail(email, ct);
            if (acc == null) return; // tránh lộ email

            var code = Random.Shared.Next(0, 10000).ToString("D4");
            await _store.SetOtpAsync(email, code, TimeSpan.FromMinutes(5), ct);

            // gửi email
            await _mail.SendAsync(email, "[ComputerSales] Your OTP",
                $@"<p>Verification code: <b style=""font-size:18px"">{code}</b></p>
                <p>Valid for 5 minutes.</p>", ct);
        }
    }
}
