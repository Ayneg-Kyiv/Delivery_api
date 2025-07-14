using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructure(configuration);

            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<ISessionDataService, SessionDataService>();

            return services;
        }
    }
}
