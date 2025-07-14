namespace Domain.Options
{
    public class ConnectionStringOptions
    {
        //Commonly used section name in appsettings.json
        public const string SectionName = "ConnectionStrings";

        //Default connection string name for the projects
        public string? IdentityDbConnection { get; set; }
        public string? ApplicationDbConnection { get; set; }
    }
}