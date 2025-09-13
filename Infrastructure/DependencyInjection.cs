using Domain;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Options;
using Infrastructure.Contexts;
using Infrastructure.MappingProfiles;
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

            services.AddDbContext<ShippingDbContext>((provider, options) =>
            {
                options.UseSqlServer(provider.GetRequiredService
                    <IOptionsSnapshot<ConnectionStringOptions>>().Value.ApplicationDbConnection);
            });

            //Base repository registration
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

            // Application services registration
            services.AddScoped<IMailService, MailTrapService>();

             //Automapper services profiles registration
            services.AddAutoMapper(cfg =>
            {
                //add all profiles here
                cfg.AddProfile(new ArticleProfile());
                cfg.AddProfile(new DeliveryOrderProfile());
                cfg.AddProfile(new LocationProfile());
                cfg.AddProfile(new DeliverySlotProfile());
                cfg.AddProfile(new TripProfile());
            });

            return services;
        }
    }
}
