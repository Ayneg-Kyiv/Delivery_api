using Api.Handlers;
using Api.Providers;
using Application;
using Application.Middleware;
using Domain.Models.Identity;
using Infrastructure;
using Infrastructure.Contexts;
using Infrastructure.Seeds;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Domain.Options;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;
using Domain.Interfaces.Repositories;
using Domain.Models.Orders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ConnectionStringOptions>(
    builder.Configuration.GetSection("ConnectionStringOptions"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi(); // Removed because AddOpenApi does not exist; AddSwaggerGen is used instead.

builder.Services.AddCors();

builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

#if DEBUG
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
#else
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
#endif
})
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;

#if DEBUG
        options.RequireHttpsMetadata = false;
#else
        options.RequireHttpsMetadata = true;
#endif

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]
                ?? throw new UnauthorizedAccessException("Missing jwt bearer key"))),
            ClockSkew = TimeSpan.FromMinutes(1),
            RequireExpirationTime = true
        };
    });

builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, DynamicRoleHandler>();

builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "Files")),
        RequestPath = "/Files"
    });

    app.UseCors(options =>
    {
        options.WithOrigins("https://localhost:3000")
            .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
            //   .AllowAnyOrigin()
            //   .AllowAnyHeader()
            //   .AllowAnyMethod();
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await ApplicationUserSeed.SeedRolesAsync(roleManager);

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        await ApplicationUserSeed.SeedUserAsync(userManager);

        // Додаємо Seed для ShippingOrder
        var shippingDbContext = services.GetRequiredService<ShippingDbContext>();
        await ShippingOrderSeed.SeedAsync(shippingDbContext);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}
 // Temporarily disabled for testing
app.UseCsrfProtection();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
