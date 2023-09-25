using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Commands.UpdateAccountDetail;
using Application.Accounts.Queries.GetAccountById;
using Application.Accounts.Queries.GetAccountByLogin;
using Application.Accounts.Queries.GetAllAccounts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class AccountController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<ActionResult<int>> Post(CreateAccountCommand command)
    {
        return await Mediator.Send(command);
    }

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

    [HttpGet]
    public async Task<IEnumerable<Account>> GetAll()
    {
        return await Mediator.Send(new GetAllAccountsQuery());
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> Delete(DeleteAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
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
}