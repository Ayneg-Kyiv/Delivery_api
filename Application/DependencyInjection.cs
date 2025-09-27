using Application.Services;
using AutoMapper;
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

            // Додаємо AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly, typeof(Infrastructure.DependencyInjection).Assembly);

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionDataService, SessionDataService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ShippingStartingPointService>();
            services.AddScoped<IShippingOrderService, ShippingOrderService>();
            services.AddScoped<IShippingOfferService, ShippingOfferService>();
            services.AddScoped<IShippingObjectService, ShippingObjectService>();
            services.AddScoped<IShippingDestinationService, ShippingDestinationService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IDeliveryRequestService, DeliveryRequestService>();
            services.AddScoped<IMessageService, MessageService>();

            return services;
        }
    }
}
