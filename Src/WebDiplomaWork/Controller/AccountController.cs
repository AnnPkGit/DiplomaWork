using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Commands.UpdateAccountDetail;
using Application.Accounts.Queries.GetAccountById;
using Application.Accounts.Queries.GetAccountsByLoginOrName;
using Application.Accounts.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class AccountController : ApiV1ControllerBase
{
    [HttpGet("by/id")]
    public async Task<AccountDto> GetById(int id)
    {
        return await Mediator.Send(new GetAccountByIdQuery(id));
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> Put(UpdateAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPatch, Authorize]
    public async Task<IActionResult> Patch(UpdateAccountDetailCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("search")]
    public async Task<IEnumerable<AccountSearchDto>> GetAccountsByLoginOrName([FromQuery] GetAccountsByLoginOrNameQuery command)
    {
        return await Mediator.Send(command);
    }
}