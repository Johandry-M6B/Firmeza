using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(bool Success, string UserId, string FirstName, string LastName, string Email, List<string> Roles)> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (false, string.Empty, string.Empty, string.Empty, string.Empty, new List<string>());
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return (false, string.Empty, string.Empty, string.Empty, string.Empty, new List<string>());
        }

        var roles = await _userManager.GetRolesAsync(user);

        return (true, user.Id, user.FirstName, user.LastName, user.Email!, roles.ToList());
    }
}
