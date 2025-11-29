using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<(bool Success, string UserId, string FirstName, string LastName, string Email, List<string> Roles)> AuthenticateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("User not found: {Email}", email);
            return (false, string.Empty, string.Empty, string.Empty, string.Empty, new List<string>());
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            Console.WriteLine($"LOGIN FAILED: {result}");
            if (result.IsLockedOut) Console.WriteLine("Reason: LockedOut");
            if (result.IsNotAllowed) Console.WriteLine("Reason: NotAllowed");
            if (result.RequiresTwoFactor) Console.WriteLine("Reason: RequiresTwoFactor");
            
            _logger.LogWarning("Invalid password for user: {Email}. Result: {Result}", email, result);
            return (false, string.Empty, string.Empty, string.Empty, string.Empty, new List<string>());
        }

        var roles = await _userManager.GetRolesAsync(user);

        return (true, user.Id, user.FirstName, user.LastName, user.Email!, roles.ToList());
    }
}
