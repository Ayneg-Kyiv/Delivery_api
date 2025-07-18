using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface IAccountService
    {
        Task<TResponse> GetUserDataAsync(HttpContext context, CancellationToken cancellationToken);

        Task<TResponse> SignupAsync(SignupDTO signup, HttpContext context);
        Task<TResponse> SigninAsync(SigninDTO signin, HttpContext context);
        Task<TResponse> ChangePasswordAsync(ChangePasswordDTO changePassword, HttpContext context);
        Task<TResponse> ChangeUserDataAsync(ChangeUserDataDTO changeUserData, HttpContext context);
        Task<TResponse> ChangeProfileImageAsync(UpdateProfileImageDTO updateProfileImage,
            HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> SignoutAsync(HttpContext context);
        Task<TResponse> RefreshSessionAsync(HttpContext context);
    }
}
