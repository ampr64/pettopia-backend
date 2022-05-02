using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace Infrastructure.Mail
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string recipientEmail, string subject, string body)
        {
            string name = _configuration.GetValue<string>("Smtp:Name");
            string userName = _configuration.GetValue<string>("Smtp:Username");
            string password = _configuration.GetValue<string>("Smtp:Password");
            string host = _configuration.GetValue<string>("Smtp:Host");
            int port = _configuration.GetValue<int>("Smtp:Port");
            bool enableSsl = _configuration.GetValue<bool>("Smtp:EnableSsl");

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(name, userName));
            mailMessage.To.Add(MailboxAddress.Parse(recipientEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(host, port, enableSsl);
                await smtpClient.AuthenticateAsync(userName, password);
                await smtpClient.SendAsync(mailMessage);
                await smtpClient.DisconnectAsync(true);
            }

        }
    }
}
