﻿using ComputerSales.Application.Interface.Interface_Email_Respository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

            using var client = new SmtpClient(host!, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            using var msg = new MailMessage(from!, toEmail, subject, htmlBody) { IsBodyHtml = true };
            await client.SendMailAsync(msg, ct);
        }
    }
}
