using Application.Auth.Commands.LoginUserUsingSession;
using Application.Auth.Commands.LogoutUser;
using Application.Auth.Commands.SendVerifyMsgByEmail;
using Application.Auth.Commands.UserEmailConfirm;
using Application.Users.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;
public class AuthController : ApiV1ControllerBase
{
    [HttpGet("confirmation/email")]
    public async Task<IActionResult> UserEmailConfirmAsync([FromQuery] int id, [FromQuery] string token)
    {
        await Mediator.Send(new UserEmailConfirmCommand(id, token));
        return NoContent();
    }
    
    [HttpPost("login")]
    public async Task<UserBriefDto> LoginUserUsingSessionAsync(LoginUserUsingSessionCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("send/email"), Authorize]
    public async Task<IActionResult> SendEmailVerifyMessage()
    {
        await Mediator.Send(new SendVerifyMsgByEmailCommand());
        return NoContent();
    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> LogoutUser()
    {
        await Mediator.Send(new LogoutUserCommand());
        return NoContent();
    }
}