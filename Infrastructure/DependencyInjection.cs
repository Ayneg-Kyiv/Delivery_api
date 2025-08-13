using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Options;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.AddDomain(configuration);

            services.AddDbContext<IdentityDbContext>((provider, options) =>
            {

                options.UseSqlServer(provider.GetRequiredService
                    <IOptionsSnapshot<ConnectionStringOptions>>().Value.IdentityDbConnection);
            });

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IMailService, MailTrapService>();

            return services;
        }
    }
}
