namespace Application.Auth.Commands.Login;

public record LoginResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);
