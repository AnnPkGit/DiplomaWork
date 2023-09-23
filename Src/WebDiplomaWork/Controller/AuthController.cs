using Application.Auth.Commands.LoginUserUsingSession;
using Application.Auth.Commands.SendVerifyMsgByEmail;
using Application.Auth.Commands.UserEmailConfirm;
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
    public async Task<IActionResult> LoginUserUsingSessionAsync(LoginUserUsingSessionCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("send/email"), Authorize]
    public async Task<IActionResult> SendEmailVerifyMessage()
    {
        await Mediator.Send(new SendVerifyMsgByEmailCommand());
        return NoContent();
    }
}