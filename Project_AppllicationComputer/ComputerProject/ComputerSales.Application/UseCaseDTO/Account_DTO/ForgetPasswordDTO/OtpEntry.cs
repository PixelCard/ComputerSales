using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO
{
    public enum OtpVerifyResult
    {
        Ok,
        NotFound,
        Expired,
        TooManyAttempts,
        Mismatch
    }

    public sealed record OtpEntry(string Hash, DateTimeOffset Exp, int Attempts, int MaxAttempts);
}
