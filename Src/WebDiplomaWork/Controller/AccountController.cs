using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Commands.UpdateAccountDetail;
using Application.Accounts.Queries.GetAccountById;
using Application.Accounts.Queries.GetAccountByLogin;
using Application.Accounts.Queries.GetAccountsByLoginOrName;
using Application.Accounts.Queries.GetAccountsWithPaginationQuery;
using Application.Accounts.Queries.Models;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class AccountController : ApiV1ControllerBase
{
    [HttpGet("by/id")]
    public async Task<Account> GetById(int id)
    {
        return await Mediator.Send(new GetAccountByIdQuery(id));
    }
    
    [HttpGet("by/login")]
    public async Task<Account> GetByLogin([FromQuery] string login)
    {
        return await Mediator.Send(new GetAccountByLoginQuery(login));
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<PaginatedList<AccountBriefDto>>> GetWithPagination([FromQuery] GetAccountsWithPaginationQuery command)
    {
        return await Mediator.Send(command);
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
    public async Task<IEnumerable<AccountBriefDto>> GetAccountsByLoginOrName([FromQuery] GetAccountsByLoginOrNameQuery command)
    {
        return await Mediator.Send(command);
    }
}