using Application.Auth.Commands.Login;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

public class AuthController : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
}
