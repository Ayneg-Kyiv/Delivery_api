using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.Identity;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class SessionDataService(IBaseRepository<SessionData> sessionRepository)
        : ISessionDataService
    {
        public async Task AddSessionAsync(HttpContext context, string token, Guid userId)
        {
            var sessionData = new SessionData
            {
                UserId = userId,
                RefreshToken = token,
                UserAgent = context.Request.Headers["User-Agent"],
                DeviceIdentificator = context.Request.Headers["Device-Id"],
            };

            sessionData.SetIpAddress(context.Connection.RemoteIpAddress);

            try
            {
                await sessionRepository.AddAsync(sessionData, default);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                throw new InvalidOperationException("Failed to add session data.", ex);
            }
        }

        public async Task<bool> IsSessionExistsAsync(HttpContext context)
        {
            return await sessionRepository.FindAsync(x => x.RefreshToken
            == context.Request.Cookies["refreshToken"], default) != null;
        }

        public async Task RemoveSessionAsync(HttpContext context)
        {
            try
            {
                var sessions = await sessionRepository.FindAsync(x => x.RefreshToken
                == context.Request.Cookies["refresh_token"], default);

                var sessionData = sessions.FirstOrDefault();

                if (sessionData != null)
                {
                    await sessionRepository.DeleteAsync(sessionData, default);
                }
                {
                    throw new InvalidOperationException("Session data not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                throw new InvalidOperationException("Failed to remove session data.", ex);
            }
        }

        public async Task UpdateSessionRefreshTokenAsync(string oldToken, string newToken)
        {
            try
            {
                var sessions = await sessionRepository.FindAsync(x => x.RefreshToken == oldToken, default);
                var sessionData = sessions.FirstOrDefault();

                if (sessionData != null)
                {
                    sessionData.RefreshToken = newToken;

                    sessionData.SetUpdatedAt();
                    sessionData.SetLastActive();

                    await sessionRepository.UpdateAsync(sessionData, default);
                }
                else
                {
                    throw new InvalidOperationException("Session data not found for the provided token.");
                }
            }
            catch (Exception ex) 
            {
                // Handle exception (e.g., log it)
                throw new InvalidOperationException("Failed to update session refresh token.", ex);
            }
        }
    }
}
