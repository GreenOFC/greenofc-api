using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Services.Otp
{
    public interface IEmailService
    {
        Task SendAsync(string emailTo, string subject, string body);
    }

    public class EmailService: IEmailService, IScopedLifetime
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IMongoRepository<EmailHistory> _emailHistoryRepository;
        private readonly EmailConfig _emailConfig;

        public EmailService(
            ILogger<EmailService> logger,
            IMongoRepository<EmailHistory> emailHistoryRepository,
            IOptions<EmailConfig> emailConfigOption)
        {
            _logger = logger;
            _emailHistoryRepository = emailHistoryRepository;
            _emailConfig = emailConfigOption.Value;
        }

        public async Task SendAsync(string emailTo, string subject, string body)
        {
            var emailHistory = new EmailHistory();
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailConfig.Sender);
                email.Sender.Name = _emailConfig.SenderName;
                email.From.Add(email.Sender);
                email.To.Add(MailboxAddress.Parse(emailTo));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                emailHistory.PayLoad = email.ToString();
                emailHistory.Email = emailTo;

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port);
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }

                emailHistory.IsSuccess = true;
                await _emailHistoryRepository.InsertOneAsync(emailHistory);
            }
            catch (Exception ex)
            {
                emailHistory.Message = ex.Message;
                await _emailHistoryRepository.InsertOneAsync(emailHistory);

                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
