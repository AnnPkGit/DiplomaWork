using Application.Auth.Commands.ConfirmPhone;
using Application.Auth.Commands.LoginUserUsingSession;
using Application.Auth.Commands.LogoutUser;
using Application.Auth.Commands.SendVerifyMsgByEmail;
using Application.Auth.Commands.UserEmailConfirm;
using Application.Common.Interfaces;
using Application.Users.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
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
    
    [HttpPost("send/sms/"), Authorize] 
    public async Task<IActionResult> SendSmsVerifyMessage()
    {
        var smsCommand = new SendSmsCommand();
        await Mediator.Send(smsCommand);
        return NoContent();

    }

    [HttpPost("confirm/phone"), Authorize] 
    public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmPhoneCommand command)
    {
        var isSuccess = await Mediator.Send(command);
        return isSuccess ? Ok("Phone verification successful") : Unauthorized("Invalid code");

    }
    
    [HttpGet("logout")]
    public async Task<IActionResult> LogoutUser()
    {
        await Mediator.Send(new LogoutUserCommand());
        return NoContent();
    }
}