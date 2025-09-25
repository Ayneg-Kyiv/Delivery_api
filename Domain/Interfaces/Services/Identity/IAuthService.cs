using Domain.Models.DTOs;
using Domain.Models.DTOs.Auth;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface IAuthService
    {
        Task<TResponse> GoogleAuthenticateWithTokenAsync(GoogleAuthRequest request, HttpContext context);
        Task<TResponse> SignupAsync(SignupDTO signup, HttpContext context);
        Task<TResponse> SigninAsync(SigninDTO signin, HttpContext context);
        Task<TResponse> SignoutAsync(HttpContext context);
        Task<TResponse> RefreshSessionAsync(HttpContext context);
        Task<TResponse> ConfirmEmailAsync(string token, string email, HttpContext context);
        Task<TResponse> ResendConfirmationEmailAsync(string email);
        // Additional Functions
        Task<TResponse> CheckIsUserExists(string email);
    }
}
