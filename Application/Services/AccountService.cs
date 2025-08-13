using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Domain.Models.Identity;
using Domain.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
                                IBaseRepository<ApplicationUser> applicationUserRepository,
                                IFileService fileService,
                                IMailService mailService) : IAccountService
    {
        public async Task<TResponse> GetUserDataAsync(HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                // Спочатку спробуємо знайти email в JwtRegisteredClaimNames.Email (стандартний JWT claim)
                var email = context.User.Claims
                    .FirstOrDefault(c => c.Type == "email")?.Value;

                // Якщо не знайдено, спробуємо в ClaimTypes.Email
                if (string.IsNullOrWhiteSpace(email))
                {
                    email = context.User.Claims
                        .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                }

                Console.WriteLine($"GetUserDataAsync - Email from claims: {email}");

                if (string.IsNullOrWhiteSpace(email))
                    return TResponse.Failure(401, "No session on this device");

                // Використовуємо UserManager замість репозиторію для обходу кешування
                var user = await userManager.FindByEmailAsync(email);

                Console.WriteLine($"User found via UserManager: {user != null}");
                if (user != null)
                {
                    Console.WriteLine($"User FirstName: '{user.FirstName}', LastName: '{user.LastName}', MiddleName: '{user.MiddleName}'");
                    Console.WriteLine($"User Email: '{user.Email}', Id: '{user.Id}'");
                }

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
    }
}
