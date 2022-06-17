using Application.Common.Interfaces;
using Application.Common.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Mail
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public SmtpEmailService(SmtpSettings settings)
        {            
            _settings = settings;
        }

        public async Task SendAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken)
        {
            var textFormat = _settings.IsBodyHtml ? TextFormat.Html : TextFormat.Plain;

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(_settings.Name, _settings.Username));
            mailMessage.To.Add(MailboxAddress.Parse(recipientEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart(textFormat)
            {
                Text = body
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl, cancellationToken);
            await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await smtpClient.SendAsync(mailMessage, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
        }
    }
}