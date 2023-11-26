using Application.Accounts.Queries.Models;
using Application.Common.Models;
using Application.Follows.Commands.FollowAccount;
using Application.Follows.Commands.UnfollowAccount;
using Application.Follows.Queries.GetFollowersByAccount;
using Application.Follows.Queries.GetFollowsByAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class FollowController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<IActionResult> FollowAccount(FollowAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete, Authorize]
    public async Task<IActionResult> UnfollowAccount(UnfollowAccountCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("followers")]
    public async Task<PaginatedList<AccountBriefDto>> GetFollowersByAccount([FromQuery] GetFollowersByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("follows")]
    public async Task<PaginatedList<AccountBriefDto>> GetFollowsByAccount([FromQuery] GetFollowsByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
}