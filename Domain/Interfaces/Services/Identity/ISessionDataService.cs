using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface ISessionDataService
    {
        Task AddSessionAsync(HttpContext context, string token, Guid userId);
        
        Task RemoveSessionAsync(HttpContext context);
        
        Task<bool> IsSessionExistsAsync(HttpContext context);

        Task UpdateSessionRefreshTokenAsync(string oldToken, string newToken);
    }
}
