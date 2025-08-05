using Domain;
using Domain.Interfaces.Repositories;
using Domain.Models.Identity;
using Domain.Models.Orders;
using Domain.Options;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
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
                    <IOptionsSnapshot<ConnectionStringOptions>>().Value.ShippingDbConnection);
            });

            // Реєстрація репозиторіїв з відповідними контекстами
            services.AddScoped<IBaseRepository<ShippingStartingPoint>>(provider =>
                new BaseRepository<ShippingStartingPoint, ShippingDbContext>(
                    provider.GetRequiredService<ShippingDbContext>()));

            services.AddScoped<IBaseRepository<ShippingOrder>>(provider =>
                new BaseRepository<ShippingOrder, ShippingDbContext>(
                    provider.GetRequiredService<ShippingDbContext>()));

            services.AddScoped<IBaseRepository<ShippingOffer>>(provider =>
                new BaseRepository<ShippingOffer, ShippingDbContext>(
                    provider.GetRequiredService<ShippingDbContext>()));

            services.AddScoped<IBaseRepository<ShippingObject>>(provider =>
                new BaseRepository<ShippingObject, ShippingDbContext>(
                    provider.GetRequiredService<ShippingDbContext>()));

            services.AddScoped<IBaseRepository<ShippingDestination>>(provider =>
                new BaseRepository<ShippingDestination, ShippingDbContext>(
                    provider.GetRequiredService<ShippingDbContext>()));

            services.AddScoped<IBaseRepository<ApplicationUser>>(provider =>
                new BaseRepository<ApplicationUser, IdentityDbContext>(
                    provider.GetRequiredService<IdentityDbContext>()));

            services.AddScoped<IBaseRepository<SessionData>>(provider =>
                new BaseRepository<SessionData, IdentityDbContext>(
                    provider.GetRequiredService<IdentityDbContext>()));

            // services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
