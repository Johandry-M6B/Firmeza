using Application.Auth.Commands.Login;
using Application.Auth.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(RegisterCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
