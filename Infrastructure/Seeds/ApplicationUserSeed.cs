using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds
{
    public class ApplicationUserSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { "SuperUser", "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            var adminUser = await userManager.FindByEmailAsync("admin123@i.ua");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin123@i.ua",
                    NormalizedUserName = "ADMIN123@I.UA",
                    Email = "admin123@i.ua",
                    NormalizedEmail = "ADMIN123@I.UA",
                    EmailConfirmed = true,
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                };

                var hasher = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin_123");

                var result = await userManager.CreateAsync(adminUser);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "SuperUser");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "User");
                }

            }

            var defaultUser = await userManager.FindByEmailAsync("default@i.ua");

            if (defaultUser == null)
            {
                defaultUser = new ApplicationUser
                {
                    UserName = "defaultUser@i.ua",
                    NormalizedUserName = "DEFAULTUSER@I.UA",
                    Email = "defaultUser@i.ua",
                    NormalizedEmail = "DEFAULTUSER@I.UA",
                    EmailConfirmed = true,
                    PhoneNumber = "0987654321",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false,
                };

                var hasher = new PasswordHasher<ApplicationUser>();
                defaultUser.PasswordHash = hasher.HashPassword(defaultUser, "defaultUser_123");

                var result = await userManager.CreateAsync(defaultUser);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
