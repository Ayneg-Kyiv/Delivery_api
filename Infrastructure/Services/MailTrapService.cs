using Domain.Interfaces.Services;
using Domain.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class MailTrapService(IOptions<SMTPServiceOptions> options) : IMailService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                System.Console.WriteLine("No recipient email provided");
                return false;
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                System.Console.WriteLine("No subject provided");
                return false;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                System.Console.WriteLine("No body provided");
                return false;
            }

            if (options.Value is null)
            {
                System.Console.WriteLine("SMTP options are not configured");
                return false;
            }

            if (string.IsNullOrWhiteSpace(options.Value.Host) || options.Value.Port <= 0 || string.IsNullOrWhiteSpace(options.Value.AccessToken) || string.IsNullOrWhiteSpace(options.Value.SenderEmail))
            {
                System.Console.WriteLine("SMTP options are incomplete");
                return false;
            }

            try
            {
                var client = new SmtpClient(options.Value.Host, options.Value.Port)
                {
                    Credentials = new NetworkCredential(options.Value.Username, options.Value.Password),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(options.Value.SenderEmail),
                    To = { new MailAddress(to) },
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public Task<bool> SubscribeUserAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
