using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(ApplicationUser user, IEnumerable<string> roles);

        Task GetRefreshTokenAsync(ApplicationUser user, IEnumerable<string> roles, HttpContext context, bool rememberMe);

        Task<string> RefreshSessionAsync(ApplicationUser user, IEnumerable<string> roles, HttpContext context);
    }
}
