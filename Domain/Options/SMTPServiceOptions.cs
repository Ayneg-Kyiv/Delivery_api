namespace Domain.Options
{
    public class SMTPServiceOptions
    {
        public const string SectionName = "SMTPService";

        public string? Host { get; set; }
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;

        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? SenderEmail { get; set; }

        public string? AccessToken { get; set; }
    }
}
