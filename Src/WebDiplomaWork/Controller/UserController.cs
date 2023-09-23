using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUserEmail;
using Application.Users.Commands.UpdateUserPassword;
using Application.Users.Commands.UpdateUserPhone;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetCurrentUser;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class UserController : ApiV1ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpGet("all"), Authorize]
    public async Task<IEnumerable<User>> Get()
    {
        return await Mediator.Send(new GetAllUsersQuery());
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
}