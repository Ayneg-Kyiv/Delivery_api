using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Identity;
using Domain.Models.DTOs;
using Domain.Models.DTOs.Auth;
using Domain.Models.DTOs.Identity;
using Domain.Models.Identity;
using Domain.Options;
using Domain.Validators;
using Google.Apis.Auth;
using Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole<Guid>> roleManager,
                             ISessionDataService sessionDataService,
                             IBaseRepository<SessionData, IdentityDbContext> sessionDataRepository,
                             ITokenService tokenService,
                             IMailService mailService,
                             IOptions<GoogleAuthOptions> googleAuthOptions) : IAuthService
    {
        private readonly GoogleAuthOptions _googleAuthOptions = googleAuthOptions.Value;

        public async Task<TResponse> GoogleAuthenticateWithTokenAsync(GoogleAuthRequest request, HttpContext context)
        {
            try
            {
                // Verify the Google ID token
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    // Get this from IConfiguration in real application

                    Audience = [ _googleAuthOptions.ClientId ] // Your client ID
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, validationSettings);

                if (payload == null)
                    return TResponse.Failure(401, "Invalid Google token");

                var email = payload.Email;
                var firstName = payload.GivenName;
                var lastName = payload.FamilyName;
                var name = payload.Name;

                // Check if user exists
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    // Create new user
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        EmailConfirmed = true // Google already verified the email
                    };

                    var result = await userManager.CreateAsync(user);

                    if (!result.Succeeded)
                        return TResponse.Failure(500, "Failed to create user from Google account");

                    // Add user role
                    if (!await roleManager.RoleExistsAsync("User"))
                        await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

                    await userManager.AddToRoleAsync(user, "User");
                }

                // Generate JWT token for the user
                var roles = await userManager.GetRolesAsync(user);

                await tokenService.GetRefreshTokenAsync(user, roles, context, true);
                
                var token = await tokenService.GetTokenAsync(user, roles);

                return TResponse.Successful(new
                {
                    id = user.Id,
                    name = user.UserName,
                    email = user.Email,
                    token,
                    roles
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Google token authentication: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred during Google authentication: {ex.Message}");
            }
        }

        public async Task<TResponse> RefreshSessionAsync(HttpContext context)
        {
            try
            {
                var refreshToken = context.Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                    return TResponse.Failure(401, "No session on this device");

                var sessionData = await sessionDataRepository.FindAsync
                    ([x => x.RefreshToken == refreshToken], default);

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
                if (!AccountDataValidator.IsValidEmail(signin.Email ?? ""))
                    return TResponse.Failure(400, "Email is incorrect");

                var user = await userManager.FindByEmailAsync(signin.Email ?? "");

                if (user == null || !await userManager.CheckPasswordAsync(user, signin.Password ?? ""))
                    return TResponse.Failure(400, "Invalid email or password.");

                var roles = await userManager.GetRolesAsync(user);

                await tokenService.GetRefreshTokenAsync(user, roles, context, signin.RememberMe);

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
                if (!AccountDataValidator.IsValidEmail(signup.Email ?? ""))
                    return TResponse.Failure(400, "Email is incorrect");

                var existingUser = await userManager.FindByEmailAsync(signup.Email ?? "");

                if (existingUser != null) return TResponse.Failure(400, "User already exists");

                var user = new ApplicationUser()
                {
                    UserName = signup.Email,
                    Email = signup.Email,
                    FirstName = signup.FirstName,
                    LastName = signup.LastName,
                    DateOfBirth = signup.BirthDate,
                    PhoneNumber = signup.PhoneNumber
                };

                (bool isValid, string? errorMessage) = await AccountDataValidator.IsPasswordValid
                                                            (user, signup.Password ?? "", userManager);

                if (!isValid) return TResponse.Failure(400, errorMessage ?? "Password is not valid");

                var account = await userManager.CreateAsync(user, signup.Password ?? "");

                if (!account.Succeeded) return TResponse.Failure(500, "Something went wrong");

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

                    if (!roleResult.Succeeded) return TResponse.Failure(500, "Something went wrong");
                }

                await userManager.AddToRoleAsync(user, "User");

                //Send Confirmation email

                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = $"https://localhost:3000/confirm-email?token={Uri.EscapeDataString(confirmationToken)}&email={Uri.EscapeDataString(user.Email!)}";

                await mailService.SendEmailAsync(
                    user.Email ?? throw new Exception("Email is null"),
                    "Email Confirmation",
                    $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>");

                return TResponse.Successful("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing up: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while signing up: {ex.Message}");
            }
        }

        public async Task<TResponse> ConfirmEmailAsync(string token, string email, HttpContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                    return TResponse.Failure(400, "Invalid token or email");

                var user = await userManager.FindByEmailAsync(email);
                if (user == null) return TResponse.Failure(404, "User not found");

                var result = await userManager.ConfirmEmailAsync(user, token);

                if (!result.Succeeded) return TResponse.Failure(500, "Failed to confirm email");

                context.Response.Redirect("https://localhost:7000/");

                return TResponse.Successful("Email confirmed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming email: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while confirming email: {ex.Message}");
            }
        }

        public async Task<TResponse> ResendConfirmationEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return TResponse.Failure(400, "Email is required");

                var user = await userManager.FindByEmailAsync(email);
                if (user == null) return TResponse.Failure(404, "User not found");

                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = $"https://localhost:3000/confirm-email?token={Uri.EscapeDataString(confirmationToken)}&email={Uri.EscapeDataString(user.Email!)}";

                await mailService.SendEmailAsync(
                    user.Email ?? throw new Exception("Email is null"),
                    "Resend Email Confirmation",
                    $"Please confirm your email by clicking this link: <a href=\"{confirmationLink}\">Confirm Email</a>");

                return TResponse.Successful("Confirmation email sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resending confirmation email: {ex.Message}");

                return TResponse.Failure(500, $"An error occurred while resending confirmation email: {ex.Message}");
            }
        }

        public async Task<TResponse> CheckIsUserExists(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return TResponse.Failure(400, "Email is required");

                var user = await userManager.FindByEmailAsync(email);
                if (user == null) return TResponse.Failure(404, "User not found");

                return TResponse.Successful(new { exists = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking user existence: {ex.Message}");
                return TResponse.Failure(500, $"An error occurred while checking user existence: {ex.Message}");
            }
        }
    }
}
