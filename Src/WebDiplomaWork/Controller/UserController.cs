using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUserEmail;
using Application.Users.Commands.UpdateUserPassword;
using Application.Users.Commands.UpdateUserPhone;
using Application.Users.Queries.GetAllUsers;
using Domain.Entity;
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

    [HttpGet, Authorize]
    public async Task<IEnumerable<User>> Get()
    {
        return await Mediator.Send(new GetAllUsersQuery());
    }

    [HttpPatch("email"), Authorize]
    public async Task<IActionResult> UpdateEmail(UpdateUserEmailCommand command)
    {
        // TODO: Implement new email validation
        await Mediator.Send(command);
        return NoContent();
    }
        
    [HttpPatch("phone"), Authorize]
    public async Task<IActionResult> ChangePhone(UpdateUserPhoneCommand command)
    {
        // TODO: Implement new phone number validation
        await Mediator.Send(command);
        return NoContent();
    }
        
    [HttpPatch("password"), Authorize]
    public async Task<IActionResult> ChangePassword(UpdateUserPasswordCommand command)
    {
        // TODO: Implement new password validation
        await Mediator.Send(command);
        return NoContent();
    }
}