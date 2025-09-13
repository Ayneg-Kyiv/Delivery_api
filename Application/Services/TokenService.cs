using Domain.Interfaces.Services.Identity;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService(ISessionDataService sessionDataService,
                              IConfiguration configuration) : ITokenService
    {
        public async Task GetRefreshTokenAsync(ApplicationUser user, IEnumerable<string> roles, HttpContext context, bool rememberMe)
        {
            try
            {
                var authorizeClaims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
                };

                authorizeClaims.AddRange(roles.Select(role =>
                    new Claim(ClaimTypes.Role, role)));

                var refreshToken = new JwtSecurityToken(
                    issuer: configuration["Jwt:ValidIssuer"],
                    audience: configuration["Jwt:ValidAudience"],
                    expires: (rememberMe ? DateTime.UtcNow.AddDays(double.Parse(configuration["Jwt:RefreshTokenExpiryDays"] ?? "30"))
                        : DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:RefreshTokenExpiryMinutes"] ?? "60"))),
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
                    Expires = (rememberMe ? DateTime.UtcNow.AddDays(double.Parse(configuration["Jwt:RefreshTokenExpiryDays"] ?? "30")) 
                        : DateTime.UtcNow.AddMinutes(double.Parse(configuration["Jwt:RefreshTokenExpiryMinutes"] ?? "60")))
                };

                var oldRefreshToken = context.Request.Cookies["refreshToken"];


                if (!string.IsNullOrEmpty(oldRefreshToken) && await sessionDataService.IsSessionExistsAsync(context))
                {
                    await sessionDataService.UpdateSessionRefreshTokenAsync(oldRefreshToken, handledRefreshToken);
                    context.Response.Cookies.Delete("refreshToken");
                }
                else
                    await sessionDataService.AddSessionAsync(context, handledRefreshToken, user.Id);

                context.Response.Cookies.Append("refreshToken", handledRefreshToken, cookieOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the refresh token: {ex.Message}");

                throw new Exception("Failed to create refresh token", ex);
            }
        }

        public async Task<string> GetTokenAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            try
            {
                var authorizeClaims = new List<Claim>
                {
                    new (JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
                };

                authorizeClaims.AddRange(roles.Select(role =>
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

                return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(Token));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating the token: {ex.Message}");

                return string.Empty;
            }
        }

        public async Task<string> RefreshSessionAsync(ApplicationUser user, IEnumerable<string> roles, HttpContext context)
        {
            try
            {
                if (!context.Request.Cookies.TryGetValue("refreshToken", out _))
                    return string.Empty;
                else
                {
                    bool rememberMe = context.Request.Cookies.ContainsKey("rememberMe") &&
                        bool.TryParse(context.Request.Cookies["rememberMe"], out var result) && result;
                    await GetRefreshTokenAsync(user, roles, context, rememberMe);
                    return await GetTokenAsync(user, roles);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while refreshing the session: " + ex.Message);

                return string.Empty;
            }
        }
    }
}
