using ComputerSales.Application.Interface.Account_Interface;
using ComputerSales.Application.Interface.Interface_ForgetPassword;
using ComputerSales.Application.Interface.UnitOfWork;
using ComputerSales.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCase.ForgetPass_UC
{
    public class ForgotResetPassword_UC
    {
        private readonly IAccountRepository _accounts;
        private readonly IForgotPasswordRespo _store;
        private readonly IUnitOfWorkApplication _uow;

        public ForgotResetPassword_UC(IAccountRepository accounts, IForgotPasswordRespo store, IUnitOfWorkApplication uow)
        {
            _accounts = accounts;
            _store = store;
            _uow = uow;
        }

        public async Task<bool> HandleAsync(string email, string resetSessionToken, string newPassword, CancellationToken ct)
        {
            // xác minh + consume token
            var ok = await _store.ValidateAndConsumeResetSessionAsync(email, resetSessionToken, ct);
            if (!ok) return false;

            var acc = await _accounts.GetAccountByEmail(email, ct);
            if (acc == null) return false;

            acc.Pass = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _accounts.UpdateAccount(acc, ct);

            await _uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
