using Application.Accounts.Queries.Models;
using Application.Common.Models;
using Application.Reactions.Commands.CreateReaction;
using Application.Reactions.Commands.UndoReaction;
using Application.Reactions.Queries.GetAllReactions;
using Application.Reactions.Queries.GetReactionsByToast;
using Application.Reactions.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class ReactionController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<IActionResult> CreateReaction(CreateReactionCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete, Authorize]
    public async Task<IActionResult> UndoReaction(UndoReactionCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("by/toast")]
    public async Task<PaginatedList<AccountBriefDto>> GetReactionsByToast
        ([FromQuery] GetReactionsByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("all")]
    public async Task<PaginatedList<ReactionBriefDto>> GetAllReactions
        ([FromQuery] GetAllReactionsQuery command)
    {
        return await Mediator.Send(command);
    }
}