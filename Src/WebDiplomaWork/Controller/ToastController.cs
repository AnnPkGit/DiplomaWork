using Application.Accounts.Queries.Models;
using Application.Common.Models;
using Application.Toasts.Commands.CreateQuote;
using Application.Toasts.Commands.CreateReply;
using Application.Toasts.Commands.CreateReToast;
using Application.Toasts.Commands.CreateToast;
using Application.Toasts.Commands.DeleteToast;
using Application.Toasts.Queries.GetManyToastById;
using Application.Toasts.Queries.GetQuotesByToast;
using Application.Toasts.Queries.GetRepliesByAccount;
using Application.Toasts.Queries.GetRepliesByToast;
using Application.Toasts.Queries.GetReToastsByToast;
using Application.Toasts.Queries.GetToastsByAccount;
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
    
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteToast(DeleteToastCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("by/account")]
    public async Task<PaginatedList<ToastBriefDto>> GetToastsByAccount
        ([FromQuery] GetToastsByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("replies/by/account")]
    public async Task<PaginatedList<ToastBriefDto>> GetRepliesByAccount
        ([FromQuery] GetRepliesByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("replies/by/toast")]
    public async Task<PaginatedList<ReplyBriefDto>> GetRepliesByToast
        ([FromQuery] GetRepliesByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("quotes/by/toast")]
    public async Task<ActionResult<PaginatedList<ToastBriefDto>>> GetQuotesByToast
        ([FromQuery] GetQuotesByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("reToasts/by/toast")]
    public async Task<ActionResult<PaginatedList<AccountBriefDto>>> GetReToastsByToast
        ([FromQuery] GetReToastsByToastQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("many/by/id")]
    public async Task<IEnumerable<ToastBriefDto>> GetManyToastsByIds
        ([FromBody] GetManyToastsByIdQuery command)
    {
        return await Mediator.Send(command);
    }
}