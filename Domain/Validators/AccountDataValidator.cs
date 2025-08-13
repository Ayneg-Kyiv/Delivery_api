using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Validators
{
    public static class AccountDataValidator
    {
        public static bool IsValidEmail(string email)
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

        public static async Task<(bool isValid, string? ErrorMessage)> IsPasswordValid
            (ApplicationUser user, string password, UserManager<ApplicationUser> userManager)
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
