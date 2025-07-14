using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services.Identity;
using Domain.Models.DTO;
using Domain.Models.DTO.Identity;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole<Guid>> roleManager,
                                ISessionDataService sessionDataService,
                                IBaseRepository<SessionData> sessionDataRepository,
                                ITokenService tokenService) : IAccountService
    {
        public Task<TResponse> ChangePasswordAsync(ChangePasswordDTO changePassword, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> ChangeUserDataAsync(ChangeUserDataDTO changeUserData, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<TResponse> RefreshSessionAsync(HttpContext context)
        {
            var refreshToken = context.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return TResponse.Failure(401, "No session on this device");

            var sessionData = await sessionDataRepository.FindAsync
                (x => x.RefreshToken == refreshToken, default);

            if (sessionData == null)
                return TResponse.Failure(401, "No session on this device");

            var user = await userManager.FindByIdAsync
                (sessionData.FirstOrDefault()?.UserId.ToString() ?? "");

            if( user == null)
                return TResponse.Failure(401, "User not found");

            var newToken = await tokenService.RefreshSessionAsync(user, context);
            
            var roles = await userManager.GetRolesAsync(user
                ?? throw new Exception("User not found"));

            return TResponse.Successful(new 
            { 
                id = user?.Id,
                name = user?.UserName,
                email = user?.Email,
                token = newToken,
                roles
            });
        }

        public async Task<TResponse> SigninAsync(SigninDTO signin, HttpContext context)
        {
            if (!IsValidEmail(signin.Email ?? ""))
                return TResponse.Failure(400, "Email is incorrect");

            var user = await userManager.FindByEmailAsync(signin.Email ?? "");

            if (user == null || !await userManager.CheckPasswordAsync(user, signin.Password ?? ""))
                return TResponse.Failure(400, "Invalid email or password.");

            await tokenService.GetRefreshTokenAsync(user, context);

            var handledToken = await tokenService.GetTokenAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            return TResponse.Successful(new {
                id = user.Id,
                name = user.UserName,
                email = user.Email,
                token = handledToken,
                roles,
            });
        }

        public async Task<TResponse> SignoutAsync(HttpContext context)
        {
            if (!await sessionDataService.IsSessionExistsAsync(context))
                return TResponse.Failure(401, "No session on this device");

            await sessionDataService.RemoveSessionAsync(context);

            //clean all cookies
            if (context.Request.Cookies != null)
            {
                foreach (var cookie in context.Request.Cookies.Keys)
                {
                    context.Response.Cookies.Delete(cookie);
                }
            }

            return TResponse.Successful("Signed out successfully");
        }

        public async Task<TResponse> SignupAsync(SignupDTO signup, HttpContext context)
        {

            if (!IsValidEmail(signup.Email ?? ""))
                return TResponse.Failure(400, "Email is incorrect");

            var existingUser = await userManager.FindByEmailAsync(signup.Email ?? "");

            if (existingUser != null)
                return TResponse.Failure(400, "User already exists");

            var user = new ApplicationUser()
            {
                UserName = signup.Email,
                Email = signup.Email
            };

            var account = await userManager.CreateAsync(user, signup.Password ?? "");

            if (!account.Succeeded)
                return TResponse.Failure(500, "Something went wrong");

            if (!await roleManager.RoleExistsAsync("User"))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

                if (!roleResult.Succeeded)
                    return TResponse.Failure(500, "Something went wrong");
            }

            await userManager.AddToRoleAsync(user, "User");

            return TResponse.Successful("Success");
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
    }
}
