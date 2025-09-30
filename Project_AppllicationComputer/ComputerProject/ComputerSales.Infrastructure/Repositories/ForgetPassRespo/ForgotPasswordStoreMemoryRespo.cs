using ComputerSales.Application.Interface.Interface_ForgetPassword;
using ComputerSales.Application.UseCaseDTO.Account_DTO.ForgetPasswordDTO;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace ComputerSales.Infrastructure.Repositories.ForgetPassRespo
{
    public class ForgotPasswordStoreMemoryRespo : IForgotPasswordRespo
    {
        private readonly IMemoryCache _cache;
        private const string OTP_PREFIX = "fp:otp:";        // key: email → OtpEntry
        private const string RS_PREFIX = "fp:rs:";         // key: email → resetToken
        private const int DEFAULT_MAX_ATTEMPTS = 5;


        public ForgotPasswordStoreMemoryRespo(IMemoryCache cache) { _cache = cache; }

        // ==== OTP ====
        public Task SetOtpAsync(string email, string code, TimeSpan ttl, CancellationToken ct)
        {
            var exp = DateTimeOffset.UtcNow.Add(ttl);
            var entry = new OtpEntry(Hash(code), exp, 0, DEFAULT_MAX_ATTEMPTS);
            _cache.Set(OTP_PREFIX + email, entry, ttl);
            return Task.CompletedTask;
        }

        public Task InvalidateOtpAsync(string email, CancellationToken ct)
        {
            _cache.Remove(OTP_PREFIX + email);
            return Task.CompletedTask;
        }


        // ==== Reset session token (opaque) ====
        public Task<string> IssueResetSessionAsync(string email, TimeSpan ttl, CancellationToken ct)
        {
            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                              .Replace('+', '-').Replace('/', '_').TrimEnd('=');
            _cache.Set(RS_PREFIX + email, raw, ttl); // 1 phiên/email; 
            return Task.FromResult(raw);
        }

        public Task<bool> ValidateAndConsumeResetSessionAsync(string email, string token, CancellationToken ct)
        {
            var key = RS_PREFIX + email;
            if (_cache.TryGetValue(key, out string? saved) && saved == token)
            {
                _cache.Remove(key);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // ==== Hash utils ====
        private string Hash(string code)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(code));
            return Convert.ToBase64String(bytes);
        }

        // Helper: verify + tăng attempts 
        public Task<OtpVerifyResult> VerifyOtpAsync(string email, string code, CancellationToken ct)
        {
            var key = OTP_PREFIX + email;

            if (!_cache.TryGetValue(key, out OtpEntry? entry))
                return Task.FromResult(OtpVerifyResult.NotFound);

            if (entry.Exp <= DateTimeOffset.UtcNow)
            {
                _cache.Remove(key);
                return Task.FromResult(OtpVerifyResult.Expired);
            }

            if (entry.Attempts >= entry.MaxAttempts)
            {
                _cache.Remove(key);
                return Task.FromResult(OtpVerifyResult.TooManyAttempts);
            }

            // So sánh constant-time: so mảng byte (designed to prevent secret information from leaking through execution time variations)
            var saved = Convert.FromBase64String(entry.Hash);
            using var sha = SHA256.Create();
            var computed = sha.ComputeHash(Encoding.UTF8.GetBytes(code));
            bool ok = CryptographicOperations.FixedTimeEquals(saved, computed);

            if (ok)
            {
                _cache.Remove(key); // consume OTP
                return Task.FromResult(OtpVerifyResult.Ok);
            }

            var rem = entry.Exp - DateTimeOffset.UtcNow;
            _cache.Set(key, entry with { Attempts = entry.Attempts + 1 }, rem);
            return Task.FromResult(OtpVerifyResult.Mismatch);
        }
    }
}
