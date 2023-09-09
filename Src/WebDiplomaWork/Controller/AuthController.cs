using Application.Auth.Commands.LoginUser;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;
public class AuthController : ApiV1ControllerBase
{
    [HttpPost("login")]
    public async Task<LoginUserResponse> LoginUserAsync(LoginUserCommand command)
    {
        return await Mediator.Send(command);
    }
}