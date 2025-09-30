
using ComputerSales.Domain.Entity;
using ComputerSales.Application.Sercurity.JWT.Enity;
using ComputerSales.Application.Sercurity.JWT.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ComputerSales.Application.Sercurity.JWT.Respository
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _opt;
        public JwtTokenGenerator(IOptions<JwtOptions> opt)
        {
            _opt = opt.Value;
        }

        public string Generate(Account account)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key)); //Key


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //Signature - HMAC SHA256 Hash


            var roleName = account.Role?.TenRole ?? "Customer";


            //Tạo 1 claims 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.IDAccount.ToString()), //Định danh Subject (
                new Claim(ClaimTypes.NameIdentifier, account.IDAccount.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("rid", account.IDRole.ToString()) // role id (custom)
            };


            //Create Token
            var token = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opt.ExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
