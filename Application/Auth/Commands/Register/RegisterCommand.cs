using MediatR;

namespace Application.Auth.Commands.Register;

public class RegisterCommand : IRequest<int>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
}
