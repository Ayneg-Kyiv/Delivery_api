using Domain.Interfaces.Services.Identity;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService(UserManager<ApplicationUser> userManager,
                              ISessionDataService sessionDataService,
                              IConfiguration configuration) : ITokenService
    {
        public async Task GetRefreshTokenAsync(ApplicationUser user, HttpContext context)
        {
            var authorizeClaims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
                };

            var UserRoles = await userManager.GetRolesAsync(user);

            authorizeClaims.AddRange(UserRoles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

            var refreshToken = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddDays(
                    double.Parse(configuration["Jwt:RefreshTokenExpiryDays"] ?? "")),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

            var handledRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(
                    double.Parse(configuration["Jwt:RefreshTokenExpiryDays"] ?? "")),
                Path = "/"
            };

            var oldRefreshToken = context.Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(oldRefreshToken))
            { 
                await sessionDataService.UpdateSessionRefreshTokenAsync(oldRefreshToken, handledRefreshToken);
                context.Response.Cookies.Delete("refreshToken");
            }
            else
                await sessionDataService.AddSessionAsync(context, handledRefreshToken, user.Id);

            context.Response.Cookies.Append("refreshToken", handledRefreshToken, cookieOptions);
        }

        public async Task<string> GetTokenAsync(ApplicationUser user)
        {
            var authorizeClaims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
                };

            var UserRoles = await userManager.GetRolesAsync(user);

            authorizeClaims.AddRange(UserRoles.Select(role =>
                new Claim(ClaimTypes.Role, role)));

            var Token = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                claims: authorizeClaims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(configuration["Jwt:ExpiryMinutes"] ?? "")),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<string> RefreshSessionAsync(ApplicationUser user, HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("refreshToken", out string? refreshToken))
                return string.Empty;
            else
            {
                await GetRefreshTokenAsync(user, context);
                return await GetTokenAsync(user);
            }
        }
    }
}
