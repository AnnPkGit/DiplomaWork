using Application.BaseToasts.Commands.DeleteToast;
using Application.BaseToasts.Queries.GetAccountBaseToasts;
using Application.BaseToasts.Queries.GetRepliesByAccount;
using Application.BaseToasts.Queries.GetToastsMarkedByAccount;
using Application.BaseToasts.Queries.GetToastsWithMediaItemsByAccount;
using Application.BaseToasts.Queries.GetToastWithContentById;
using Application.BaseToasts.Queries.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class BaseToastController : ApiV1ControllerBase
{
    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteToast(DeleteBaseToastCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet("by/account")]
    public async Task<PaginatedList<BaseToastDto>> GetAccountBaseToasts([FromQuery] GetAccountBaseToastsQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("marked/by/account")]
    public async Task<PaginatedList<BaseToastWithContentBriefDto>> GetToastsMarkedByAccount
        ([FromQuery] GetToastsMarkedByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("replies/by/account")]
    public async Task<PaginatedList<BaseToastDto>> GetRepliesByAccount
        ([FromQuery] GetRepliesByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("withContent/by/id")]
    public async Task<BaseToastWithContentDto> GetToastWithContentById
        ([FromQuery] GetToastWithContentByIdQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("withMediaItems/by/account")]
    public async Task<PaginatedList<BaseToastWithContentDto>> GetToastWithMediaItemsByAccount
        ([FromQuery] GetToastsWithMediaItemsByAccountQuery command)
    {
        return await Mediator.Send(command);
    }
}