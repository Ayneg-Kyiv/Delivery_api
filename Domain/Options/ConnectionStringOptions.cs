namespace Domain.Options
{
    public class ConnectionStringOptions
    {
        //Commonly used section name in appsettings.json
        public const string SectionName = "ConnectionStrings";
        public string? ShippingDbConnection { get; set; }
        //connection strings for different databases
        public string? IdentityDbConnection { get; set; }
        public string? ApplicationDbConnection { get; set; }
    }
}