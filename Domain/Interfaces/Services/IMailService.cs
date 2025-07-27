namespace Domain.Interfaces.Services
{
    public interface IMailService
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
