using Application.Common.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IIdentityService identityService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _identityService = identityService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.AuthenticateAsync(request.Email, request.Password);

        if (!result.Success)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var userId = Guid.Parse(result.UserId);
        var token = _jwtTokenGenerator.GenerateToken(userId, result.FirstName, result.LastName, result.Email, result.Roles);

        return new LoginResponse(
            userId,
            result.FirstName,
            result.LastName,
            result.Email,
            token);
    }
}
