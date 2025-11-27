using Application.Auth.Commands.Login;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string UserId, string FirstName, string LastName, string Email, List<string> Roles)> AuthenticateAsync(string email, string password);
}
