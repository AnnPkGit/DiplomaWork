using Application.Common.Models;
using Application.Toasts.Commands.CreateToast;
using Application.Toasts.Queries.GetAllToasts;
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
    
    [HttpGet("all")]
    public async Task<PaginatedList<ToastBriefDto>> GetAllToasts
        ([FromQuery] GetAllToastsQuery command)
    {
        return await Mediator.Send(command);
    }
}