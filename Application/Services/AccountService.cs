using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Identity;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole<Guid>> roleManager,
                                ISessionDataService sessionDataService,
                                IBaseRepository<ApplicationUser> applicationUserRepository,
                                IBaseRepository<SessionData> sessionDataRepository,
                                ITokenService tokenService,
                                IFileService fileService) : IAccountService
    {
        public async Task<TResponse> GetUserDataAsync(HttpContext context, CancellationToken cancellationToken)
        {
            try
            {
                var email = context.User.Claims
                    .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                if (string.IsNullOrWhiteSpace(email))
                    return TResponse.Failure(401, "No session on this device");

                var user = await applicationUserRepository.FindAsync
                    (x => x.Email == email, cancellationToken);

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

                (bool isValid, string? errorMessage) = await IsPasswordValid(user, changePassword.NewPassword);

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

        public async Task<TResponse> RefreshSessionAsync(HttpContext context)
        {
            try
            {
                var refreshToken = context.Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken)) return TResponse.Failure(401, "No session on this device");

                var sessionData = await sessionDataRepository.FindAsync
                    (x => x.RefreshToken == refreshToken, default);

                if (sessionData == null) return TResponse.Failure(401, "No session on this device");

                var user = await userManager.FindByIdAsync
                    (sessionData.FirstOrDefault()?.UserId.ToString() ?? "");

                if (user == null) return TResponse.Failure(401, "User not found");

                var roles = await userManager.GetRolesAsync(user
                    ?? throw new Exception("User not found"));

                var newToken = await tokenService.RefreshSessionAsync(user, roles, context);

                if (string.IsNullOrEmpty(newToken))
                    return TResponse.Failure(500, "Failed to refresh session");

                return TResponse.Successful(new
                {
                    id = user?.Id,
                    name = user?.UserName,
                    email = user?.Email,
                    token = newToken,
                    roles
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing session: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while refreshing session: {ex.Message}");
            }
        }

        public async Task<TResponse> SigninAsync(SigninDTO signin, HttpContext context)
        {
            try
            {
                if (!IsValidEmail(signin.Email ?? ""))
                    return TResponse.Failure(400, "Email is incorrect");

                var user = await userManager.FindByEmailAsync(signin.Email ?? "");

                if (user == null || !await userManager.CheckPasswordAsync(user, signin.Password ?? ""))
                    return TResponse.Failure(400, "Invalid email or password.");

                var roles = await userManager.GetRolesAsync(user);

                await tokenService.GetRefreshTokenAsync(user, roles, context);

                var handledToken = await tokenService.GetTokenAsync(user, roles);

                return TResponse.Successful(new
                {
                    id = user.Id,
                    name = user.UserName,
                    email = user.Email,
                    token = handledToken,
                    roles,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing in: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while signing in: {ex.Message}");
            }
        }

        public async Task<TResponse> SignoutAsync(HttpContext context)
        {
            try
            {
                if (!await sessionDataService.IsSessionExistsAsync(context))
                    return TResponse.Failure(401, "No session on this device");

                await sessionDataService.RemoveSessionAsync(context);

                //clean all cookies
                if (context.Request.Cookies != null)
                {
                    foreach (var cookie in context.Request.Cookies.Keys)
                        context.Response.Cookies.Delete(cookie);
                }

                return TResponse.Successful("Signed out successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing out: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while signing out: {ex.Message}");
            }
        }

        public async Task<TResponse> SignupAsync(SignupDTO signup, HttpContext context)
        {
            try
            {
                if (!IsValidEmail(signup.Email ?? "")) return TResponse.Failure(400, "Email is incorrect");

                var existingUser = await userManager.FindByEmailAsync(signup.Email ?? "");

                if (existingUser != null) return TResponse.Failure(400, "User already exists");

                var user = new ApplicationUser()
                {
                    UserName = signup.Email,
                    Email = signup.Email
                };

                (bool isValid, string? errorMessage) = await IsPasswordValid(user, signup.Password ?? "");

                if (!isValid) return TResponse.Failure(400, errorMessage ?? "Password is not valid");

                var account = await userManager.CreateAsync(user, signup.Password ?? "");

                if (!account.Succeeded) return TResponse.Failure(500, "Something went wrong");

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

                    if (!roleResult.Succeeded) return TResponse.Failure(500, "Something went wrong");
                }

                await userManager.AddToRoleAsync(user, "User");

                return TResponse.Successful("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing up: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while signing up: {ex.Message}");
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                return (new System.Net.Mail.MailAddress(email).Address == email);
            }
            catch
            {
                return false;
            }
        }

        private async Task<(bool isValid, string? ErrorMessage)> IsPasswordValid
            (ApplicationUser user, string password)
        {
            var validators = userManager.PasswordValidators;

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(userManager, user, password);

                if (!result.Succeeded) return (false, result.Errors.FirstOrDefault()?.Description);
            }

            return (true, null);
        }
    }
}
