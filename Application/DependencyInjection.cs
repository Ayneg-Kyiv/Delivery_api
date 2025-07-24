using Application.Services;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Identity;
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

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionDataService, SessionDataService>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
