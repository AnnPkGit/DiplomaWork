using Application.Toasts.Commands.CreateToast;
using Application.Toasts.Queries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class ToastController : ApiV1ControllerBase
{
    [HttpPost, Authorize]
    public async Task<ToastDto> CreateToast(CreateToastCommand command)
    {
        return await Mediator.Send(command);
    }
}