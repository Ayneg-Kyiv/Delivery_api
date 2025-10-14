using Application.DTOs.Vehicles;
using AutoMapper;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Domain.Models.Identity;
using Domain.Models.Vehicles;
using Domain.Validators;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
                                IMapper mapper,
                                IBaseRepository<ApplicationUser, IdentityDbContext> applicationUserRepository,
                                IBaseRepository<Vehicle, ShippingDbContext> vehicleRepository,
                                IBaseRepository<DriverApplication, ShippingDbContext> driverApplicationRepository,
                                IFileService fileService,
                                IMailService mailService) : IAccountService
    {
        public async Task<TResponse> GetUserDataAsync(HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                var email = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(email))
                    return TResponse.Failure(401, "No session on this device");

                var user = await applicationUserRepository.FindAsync
                    ([x => x.Email == email], cancellationToken);

                var mappedUser = mapper.Map<IEnumerable<GetMainUserInfoDto>>(user);

                return TResponse.Successful(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user data: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while getting user data: {ex.Message}");
            }
        }

        public async Task<TResponse> ChangePasswordAsync(ChangePasswordDTO changePassword, HttpContext context)
        {
            try
            {
                // Validate input
                if (changePassword == null) return TResponse.Failure(400, "Invalid request data");

                if (string.IsNullOrWhiteSpace(changePassword.CurrentPassword) ||
                    string.IsNullOrWhiteSpace(changePassword.NewPassword))
                    return TResponse.Failure(400, "Old and new passwords are required");

                var user = await userManager.FindByEmailAsync(changePassword.Email);
                if (user == null) return TResponse.Failure(404, "User not found");

                (bool isValid, string? errorMessage) = await AccountDataValidator.IsPasswordValid
                                                            (user, changePassword.NewPassword, userManager);

                if (!isValid)
                    return TResponse.Failure(400, errorMessage ?? "Password is not valid");

                // Change password
                var result = await userManager.ChangePasswordAsync(
                    user, changePassword.CurrentPassword, changePassword.NewPassword);

                if (!result.Succeeded)
                {
                    var errorMsg = string.Join("; ", result.Errors.Select(e => e.Description));
                    return TResponse.Failure(400, $"Password change failed: {errorMsg}");
                }

                return TResponse.Successful("Password changed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing password: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while changing password: {ex.Message}");
            }
        }

        public async Task<TResponse> ChangeUserDataAsync(ChangeUserDataDTO changeUserData, HttpContext context)
        {
            try
            {
                // Validate input
                if (changeUserData == null)
                    return TResponse.Failure(400, "Invalid request data");

                if (string.IsNullOrWhiteSpace(changeUserData.Email))
                    return TResponse.Failure(400, "Email is required");

                var user = await userManager.FindByEmailAsync(changeUserData.Email);
                if (user == null)
                    return TResponse.Failure(404, "User not found");

                // Update fields if provided
                if (!string.IsNullOrWhiteSpace(changeUserData.FirstName))
                    user.FirstName = changeUserData.FirstName;

                if (!string.IsNullOrWhiteSpace(changeUserData.MiddleName))
                    user.MiddleName = changeUserData.MiddleName;

                if (!string.IsNullOrWhiteSpace(changeUserData.LastName))
                    user.LastName = changeUserData.LastName;

                if (changeUserData.DateOfBirth.HasValue)
                    user.DateOfBirth = changeUserData.DateOfBirth;

                if (!string.IsNullOrWhiteSpace(changeUserData.AboutMe))
                    user.AboutMe = changeUserData.AboutMe;

                // Save changes
                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errorMsg = string.Join("; ", result.Errors.Select(e => e.Description));
                    return TResponse.Failure(400, $"Failed to update user data: {errorMsg}");
                }

                return TResponse.Successful("User data updated successfully");
            }
            catch (Exception ex)
            {
                return TResponse.Failure(500, $"An error occurred while updating user data: {ex.Message}");
            }
        }

        public async Task<TResponse> ChangeProfileImageAsync(UpdateProfileImageDTO updateProfileImage, HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                // Validate input
                if (updateProfileImage == null || updateProfileImage.Image == null)
                    return TResponse.Failure(400, "Invalid request data");

                var user = await userManager.FindByEmailAsync(updateProfileImage.Email);

                if (user == null) return TResponse.Failure(404, "User not found");

                var fileName = await fileService.SaveFileAsync(updateProfileImage.Image, cancellationToken);

                if (string.IsNullOrEmpty(fileName))
                    return TResponse.Failure(500, "Failed to save profile image");

                user.ImagePath = fileName;

                // Save changes
                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    var errorMsg = string.Join("; ", result.Errors.Select(e => e.Description));
                    return TResponse.Failure(400, $"Failed to update profile image: {errorMsg}");
                }

                return TResponse.Successful("Profile image updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating profile image: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while updating profile image: {ex.Message}");
            }
        }

        public async Task<TResponse> ForgotPasswordAsync(ForgotPasswordDTO forgotPassword, HttpContext context)
        {
            try
            {
                if (forgotPassword == null || string.IsNullOrWhiteSpace(forgotPassword.Email))
                    return TResponse.Failure(400, "Invalid request data");

                var user = await userManager.FindByEmailAsync(forgotPassword.Email);

                if (user == null)
                    return TResponse.Failure(401, "User not found");

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                if (string.IsNullOrWhiteSpace(token))
                    return TResponse.Failure(500, "Failed to generate password reset token");

                var resetLink = $"https://localhost:3000/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

                var result = await mailService.SendEmailAsync(
                    user.Email ?? throw new Exception("User email is null"),
                    "Password Reset Request",
                    $"You requested a password reset. Click the link below to reset your password:\n{resetLink}");

                if (!result) return TResponse.Failure(500, "Failed to send password reset email");

                return TResponse.Successful("Password reset email sent successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ForgotPasswordAsync: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while processing the forgot password request: {ex.Message}");
            }
        }

        public async Task<TResponse> ResetPasswordAsync(ResetPasswordDTO resetPassword, HttpContext context)
        {
            try
            {
                if (resetPassword == null || string.IsNullOrWhiteSpace(resetPassword.Email) ||
                    string.IsNullOrWhiteSpace(resetPassword.Token)
                    || string.IsNullOrWhiteSpace(resetPassword.NewPassword))
                    return TResponse.Failure(400, "Invalid request data");

                var user = await userManager.FindByEmailAsync(resetPassword.Email);

                if (user == null) return TResponse.Failure(404, "User not found");

                (bool isValid, string? errorMessage) = await AccountDataValidator.IsPasswordValid(user, resetPassword.NewPassword, userManager);

                if (!isValid) return TResponse.Failure(400, errorMessage);

                var result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);

                if (!result.Succeeded)
                {
                    var errorMsg = string.Join("; ", result.Errors.Select(e => e.Description));
                    return TResponse.Failure(400, $"Failed to reset password: {errorMsg}");
                }

                return TResponse.Successful("Password reset successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetPasswordAsync: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while resetting the password: {ex.Message}");
            }
        }

        public async Task<TResponse> AddVehicleAsync(CreateVehicleDto createVehicle, HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                var id = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(id))
                    return TResponse.Failure(401, "No session on this device");

                if (createVehicle == null)
                    return TResponse.Failure(400, "Invalid request data");

                var user = await userManager.FindByEmailAsync(id);
                if (user == null)
                    return TResponse.Failure(404, "User not found");
                
                var vehicle = mapper.Map<Vehicle>(createVehicle);
                
                vehicle.OwnerId = user.Id;

                if (createVehicle.ImageFront != null)
                {
                    var imagePath = await fileService.SaveFileAsync(createVehicle.ImageFront, cancellationToken);
                    if (string.IsNullOrEmpty(imagePath))
                        return TResponse.Failure(500, "Failed to save vehicle image");
                    vehicle.ImagePath = imagePath;
                }

                if (createVehicle.ImageBack != null)
                {
                    var imagePathBack = await fileService.SaveFileAsync(createVehicle.ImageBack, cancellationToken);
                    if (string.IsNullOrEmpty(imagePathBack))
                        return TResponse.Failure(500, "Failed to save vehicle back image");
                    vehicle.ImagePathBack = imagePathBack;
                }

                var createdVehicle = await vehicleRepository.AddAsync(vehicle, cancellationToken);

                if(createdVehicle == false)
                    return TResponse.Failure(500, "Failed to add vehicle");

                var driverApplication = new DriverApplication
                {
                    UserId = user.Id,
                    VehicleId = vehicle.Id
                };

                var createdDriverApp = await driverApplicationRepository.AddAsync(driverApplication, cancellationToken);

                if (createdDriverApp == false)
                    return TResponse.Failure(500, "Failed to create driver application");

                return TResponse.Successful("Vehicle added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding vehicle: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while adding the vehicle: {ex.Message}");
            }
        }

        public async Task<TResponse> UpdateVehicleAsync(UpdateVehicleDto updateVehicle, HttpContext context, CancellationToken cancellationToken)
        {
            if (updateVehicle == null)
                return TResponse.Failure(400, "");

            var vehicleArr = await vehicleRepository.FindAsync([v => v.Id == updateVehicle.Id], cancellationToken);
            var vehicle = vehicleArr.FirstOrDefault();

            if (vehicle == null)
                return TResponse.Failure(404, "Vehicle not found");

            vehicle.Brand = updateVehicle.Brand ?? vehicle.Brand;
            vehicle.Model = updateVehicle.Model ?? vehicle.Model;
            vehicle.Type = updateVehicle.Type ?? vehicle.Type;

            vehicle.NumberPlate = updateVehicle.NumberPlate ?? vehicle.NumberPlate;
            vehicle.Color = updateVehicle.Color ?? vehicle.Color;

            return TResponse.Successful(true, "Vehicle edited successfully");
        }

        public async Task<TResponse> DeleteVehicleAsync(Guid id, CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
                return TResponse.Failure(400, "Invalid vehicle ID");

            var vehicleArr = await vehicleRepository.FindAsync([v => v.Id == id], cancellationToken);
            var vehicle = vehicleArr.FirstOrDefault();

            if (vehicle == null)
                return TResponse.Failure(404, "Vehicle not found");

            if (!string.IsNullOrWhiteSpace(vehicle.ImagePath))
            {
                var fileDeleted = fileService.DeleteFileAsync(vehicle.ImagePath);

                if (!fileDeleted)
                    return TResponse.Failure(500, "Failed to delete vehicle image");
            }
            if (!string.IsNullOrWhiteSpace(vehicle.ImagePathBack))
            {
                var fileDeleted = fileService.DeleteFileAsync(vehicle.ImagePathBack);
             
                if (!fileDeleted)
                    return TResponse.Failure(500, "Failed to delete vehicle back image");
            }

            if(vehicle.IsApproved)
            {
                var driverApplicationArr = await driverApplicationRepository.FindAsync([da => da.VehicleId == vehicle.Id], cancellationToken);
                var driverApplication = driverApplicationArr.FirstOrDefault();
                if (driverApplication != null)
                {
                    var deleteDriverAppResult = await driverApplicationRepository.DeleteAsync(driverApplication, cancellationToken);
                    if (!deleteDriverAppResult)
                        return TResponse.Failure(500, "Failed to delete associated driver application");
                }
            }

            // Fix CS0136: use a different variable name for the result
            var deleteResult = await vehicleRepository.DeleteAsync(vehicle, cancellationToken);
            if (!deleteResult)
                return TResponse.Failure(500, "Failed to delete vehicle");

            return TResponse.Successful("Vehicle deleted successfully");
        }

        public async Task<TResponse> GetUserVehiclesAsync(HttpContext context, CancellationToken cancellationToken)
        {
            var email = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(string.IsNullOrEmpty(email))
                return TResponse.Failure(403, "Access forbidden");

            var user = await userManager.FindByEmailAsync(email ?? "");
            
            if (user == null)
                return TResponse.Failure(400, "Failed to retrieve user");
            
            var vehicles = await vehicleRepository.FindAsync([v => v.OwnerId == user.Id], cancellationToken);

            return TResponse.Successful(vehicles, "User vehicles retrieved successfully");
        }

        public async Task<TResponse> GetUserApprovedVehiclesAsync(HttpContext context, CancellationToken cancellationToken)
        {
            var email = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email))
                return TResponse.Failure(403, "Access forbidden");

            var user = await userManager.FindByEmailAsync(email ?? "");

            if (user == null)
                return TResponse.Failure(400, "Failed to retrieve user");

            var vehicles = await vehicleRepository.FindAsync([v => v.OwnerId == user.Id, v => v.IsApproved], cancellationToken);

            return TResponse.Successful(vehicles, "User vehicles retrieved successfully");
        }

        public async Task<TResponse> ReturnDriverRequiredData(HttpContext context, CancellationToken cancellationToken)
        {
            var email = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if ( string.IsNullOrEmpty(email))
                return TResponse.Failure(403, "Access forbidden");

            var user = await userManager.FindByEmailAsync(email ?? "");

            if (user == null)
                return TResponse.Failure(400, "Failed to retrieve user");

            var driverPhone = user.PhoneNumber;
            var driverImage = user.DriverLicenseImagePath != null;
            var driverProfileImage = user.ImagePath != null;

            return TResponse.Successful(new { driverPhone, driverImage, driverProfileImage }, "Driver required data retrieved successfully");
        }

        public async Task<TResponse> SetDriverRequiredData(string? phoneNumber, IFormFile? Image, IFormFile? profileImage, HttpContext context, CancellationToken cancellationToken)
        {
            var email = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (email == Guid.Empty.ToString())
                return TResponse.Failure(403, "Access forbidden");

            var user = await userManager.FindByEmailAsync(email ?? "");

            if (user == null)
                return TResponse.Failure(400, "Failed to retrieve user");

            if(phoneNumber != null)
                user.PhoneNumber = phoneNumber;
            
            if(Image != null)
                user.DriverLicenseImagePath = await fileService.SaveFileAsync(Image, cancellationToken);

            if (profileImage != null)
                user.ImagePath = await fileService.SaveFileAsync(profileImage, cancellationToken);

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return TResponse.Failure(500, "Failed to update user data");

            return TResponse.Successful("Driver required data set successfully");
        }

        public async Task<TResponse> GetShortUserDataAsync(Guid id, CancellationToken cancellationToken)
        {
            var userArr = await applicationUserRepository.FindAsync([u => u.Id == id], cancellationToken);

            if(userArr == null || !userArr.Any())
                return TResponse.Failure(404, "User not found");

            var user = userArr.First();

            var shortData = mapper.Map<GetApplicationUserForTripDto>(user);

            return TResponse.Successful(shortData, "Short user data retrieved successfully");
        }
    }
}
