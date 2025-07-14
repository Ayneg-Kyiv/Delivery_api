using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(ApplicationUser user);

        Task GetRefreshTokenAsync(ApplicationUser user, HttpContext context);
        
        Task<string> RefreshSessionAsync(ApplicationUser user, HttpContext context);
    }
}
