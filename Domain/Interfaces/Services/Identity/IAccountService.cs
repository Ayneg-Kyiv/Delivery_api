using Application.DTOs.Vehicles;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.Services.Identity
{
    public interface IAccountService
    {
        Task<TResponse> GetUserDataAsync(HttpContext context, CancellationToken cancellationToken);

        Task<TResponse> ChangePasswordAsync(ChangePasswordDTO changePassword, HttpContext context);
        Task<TResponse> ChangeUserDataAsync(ChangeUserDataDTO changeUserData, HttpContext context);
        Task<TResponse> ChangeProfileImageAsync(UpdateProfileImageDTO updateProfileImage,
            HttpContext context, CancellationToken cancellationToken);

        Task<TResponse> ForgotPasswordAsync(ForgotPasswordDTO forgotPassword, HttpContext context);
        Task<TResponse> ResetPasswordAsync(ResetPasswordDTO resetPassword, HttpContext context);

        Task<TResponse> AddVehicleAsync(CreateVehicleDto createVehicle, HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> UpdateVehicleAsync(UpdateVehicleDto updateVehicle, HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken);
        Task<TResponse> GetUserVehiclesAsync(HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> ReturnDriverRequiredData(HttpContext context, CancellationToken cancellationToken);
        Task<TResponse> SetDriverRequiredData(string phoneNumber, IFormFile Image, HttpContext context, CancellationToken cancellationToken);
    }
}
