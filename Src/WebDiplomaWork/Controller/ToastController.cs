using Application.Common.Models;
using Application.Toasts.Commands.CreateQuote;
using Application.Toasts.Commands.CreateReply;
using Application.Toasts.Commands.CreateReToast;
using Application.Toasts.Commands.CreateToast;
using Application.Toasts.Commands.DeleteToast;
using Application.Toasts.Commands.UndoReToast;
using Application.Toasts.Queries.GetCurrentUserToasts;
using Application.Toasts.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class ToastController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<ToastBriefDto> CreateToast(CreateToastCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost("reply"), Authorize]
    public async Task<ToastBriefDto> CreateReply(CreateReplyCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost("quote"), Authorize]
    public async Task<ToastBriefDto> CreateQuote(CreateQuoteCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpPost("reToast"), Authorize]
    public async Task<ToastBriefDto> CreateReToast(CreateReToastCommand command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("pagination"), Authorize]
    public async Task<ActionResult<PaginatedList<ToastBriefDto>>> GetCurrentUserToastsWithPagination
        ([FromQuery]  GetCurrentUserToastsQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteToast(DeleteToastCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("reToast/undo"), Authorize]
    public async Task<IActionResult> UndoReToast(UndoReToastCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}