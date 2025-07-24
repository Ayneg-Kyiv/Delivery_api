using Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        // Static method that adds the domain services to the IServiceCollection
        public static IServiceCollection AddDomain(this IServiceCollection services,
                                                         IConfiguration configuration)
        {
            services.Configure<ConnectionStringOptions>(configuration
                .GetSection(ConnectionStringOptions.SectionName));

            services.Configure<SMTPServiceOptions>(configuration
                .GetSection(SMTPServiceOptions.SectionName));

            return services;
        }
    }
}
