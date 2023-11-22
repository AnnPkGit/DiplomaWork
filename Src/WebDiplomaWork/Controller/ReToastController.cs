using Application.Accounts.Queries.Models;
using Application.Common.Models;
using Application.ReToasts.Commands.CreateReToast;
using Application.ReToasts.Commands.DeleteReToastByToastId;
using Application.ReToasts.Queries.GetAllReToasts;
using Application.ReToasts.Queries.GetReToastsByToast;
using Application.ReToasts.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class ReToastController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<ReToastDto> CreateReToast(CreateReToastCommand command)
    {
           return await Mediator.Send(command);
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteReToastByToastId(DeleteReToastByToastIdCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("all")]
    public async Task<PaginatedList<ReToastDto>> GetAllReToasts
        ([FromQuery] GetAllReToastsQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/toast")]
    public async Task<PaginatedList<AccountBriefDto>> GetReToastsByToast
        ([FromQuery] GetReToastsByToastQuery command)
    {
        return await Mediator.Send(command);
    }
}