using Firmeza.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.Web.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            var adminRole = new ApplicationRole
            {
                Name = UserRoles.Admin,
                Description = "Administrator role with full permissions"
            };
            var roleResult = await roleManager.CreateAsync(adminRole);
            if (!roleResult.Succeeded)
            {
                Console.WriteLine($"Rol '{UserRoles.Admin}'");
            }
            else
            {
                Console.WriteLine($"Error creating role '{UserRoles.Admin}':");
                foreach (var error in roleResult.Errors)
                {
                    Console.WriteLine($"-{error.Description}");
                }
                return;
            }
        }

        var adminEmail = "admin@firmeza.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "System",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = "3238817902",
                DocumentNumber = "1001877889"
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123*");
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(adminUser, [UserRoles.Admin]);
                Console.WriteLine("User admin created successfully.");
                Console.WriteLine($"Email: '{adminEmail}'");
                Console.WriteLine("Password: 'Admin123*'");
            }
            else
            {
                Console.WriteLine("Error creating the admin user:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($" - {error.Description}");
                }
            }
        }
        else
        {
            var isInRole = await userManager.IsInRoleAsync(adminUser, UserRoles.Admin);
            if (isInRole)
            {
                await userManager.AddToRolesAsync(adminUser, [UserRoles.Admin]);
                Console.WriteLine($"Rol the admin assigned to {adminEmail}.");
            }
        }
    }
}

