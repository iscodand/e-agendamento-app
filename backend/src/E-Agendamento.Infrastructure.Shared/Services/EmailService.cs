using E_Agendamento.Application.Contracts.Services;
using E_Agendamento.Application.DTOs.Email;
using E_Agendamento.Application.Exceptions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace E_Agendamento.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                string email = Environment.GetEnvironmentVariable("EMAIL");
                string password = Environment.GetEnvironmentVariable("PASS");
                string host = Environment.GetEnvironmentVariable("EMAIL_HOST");
                _ = int.TryParse(Environment.GetEnvironmentVariable("EMAIL_PORT"), out int port);

                BodyBuilder builder = new()
                {
                    HtmlBody = emailRequest.Body
                };

                MimeMessage mailMessage = new()
                {
                    Sender = new MailboxAddress(email, emailRequest.From ?? email),
                    Subject = emailRequest.Subject,
                    Body = builder.ToMessageBody(),
                };
                mailMessage.To.Add(MailboxAddress.Parse(emailRequest.To));

                using SmtpClient smtp = new();
                smtp.Connect(host, port, SecureSocketOptions.StartTls);
                smtp.Authenticate(email, password);
                await smtp.SendAsync(mailMessage);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
    }
}