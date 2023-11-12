using Application.Common.Models;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RegistrateUserWithAccount;
using Application.Users.Commands.UpdateUserEmail;
using Application.Users.Commands.UpdateUserPassword;
using Application.Users.Commands.UpdateUserPhone;
using Application.Users.Queries.GetCurrentUser;
using Application.Users.Queries.GetUsersWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class UserController : ApiV1ControllerBase
{
    [HttpPost("registration")]
    public async Task<IActionResult> RegistrateUserAndAccount(RegistrateUserAndAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("pagination"), Authorize]
    public async Task<ActionResult<PaginatedList<UserBriefDto>>> GetUsersWithPagination([FromQuery] GetUsersWithPaginationQuery command)
    {
        return await Mediator.Send(command);
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
}