using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RegistrateUserWithAccount;
using Application.Users.Commands.UpdateUserEmail;
using Application.Users.Commands.UpdateUserMuteNotificationOptions;
using Application.Users.Commands.UpdateUserPassword;
using Application.Users.Commands.UpdateUserPhone;
using Application.Users.Queries.GetCurrentUser;
using Application.Users.Queries.GetCurrentUserMuteNotificationOptions;
using Application.Users.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

public class UserController : ApiV1ControllerBase
{
    [HttpPost("registration")]
    public async Task<IActionResult> RegistrateUserAndAccount(RegistrateUserAndAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet, Authorize]
    public async Task<UserBriefDto> GetCurrentUser()
    {
        return await Mediator.Send(new GetCurrentUserQuery());
    }

    [HttpPatch("email"), Authorize]
    public async Task<IActionResult> UpdateEmail(UpdateUserEmailCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
        
    [HttpPatch("phone"), Authorize]
    public async Task<IActionResult> ChangePhone(UpdateUserPhoneCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
        
    [HttpPatch("password"), Authorize]
    public async Task<IActionResult> ChangePassword(UpdateUserPasswordCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> Delete()
    {
        await Mediator.Send(new DeleteUserCommand());
        return NoContent();
    }
    
    [HttpPatch("notifications/options"), Authorize]
    public async Task<IActionResult> UpdateMuteNotificationOptions(UpdateUserMuteNotificationOptionsCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("notifications/options"), Authorize]
    public async Task<IEnumerable<MuteNotificationOptionDto>> GetCurrentUserMuteNotificationOptions()
    {
        return await Mediator.Send(new GetCurrentUserMuteNotificationOptionsQuery());
    }
}