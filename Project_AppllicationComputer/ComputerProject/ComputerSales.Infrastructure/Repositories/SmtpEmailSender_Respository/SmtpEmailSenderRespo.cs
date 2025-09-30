using ComputerSales.Application.Interface.Interface_Email_Respository;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ComputerSales.Infrastructure.Repositories.SmtpEmailSender_Respository
{
    internal class SmtpEmailSenderRespo : IEmailSender
    {
        private readonly IConfiguration _cfg;

        public SmtpEmailSenderRespo(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
        {
            var host = _cfg["Smtp:Host"];
            var port = int.Parse(_cfg["Smtp:Port"] ?? "587");
            var user = _cfg["Smtp:User"];
            var pass = _cfg["Smtp:Pass"];
            var from = _cfg["Smtp:From"];
            try
            {
                using var client = new SmtpClient(host!, port)
                {
                    EnableSsl = true,                  
                    UseDefaultCredentials = false,      
                    Credentials = new NetworkCredential(user, pass),
                    Timeout = 15000
                };

                using var msg = new MailMessage(from!, toEmail, subject, htmlBody) { IsBodyHtml = true };

                await client.SendMailAsync(msg, ct);
            }
            catch (SmtpException ex)
            {
                // Log thật chi tiết
                Console.WriteLine($"SMTP ERROR: {ex.StatusCode} - {ex.Message}");
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
