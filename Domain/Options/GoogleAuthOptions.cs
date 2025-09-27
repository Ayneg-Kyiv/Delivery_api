namespace Domain.Options
{
    public class GoogleAuthOptions
    {
        public const string SectionName = "Google";
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}
