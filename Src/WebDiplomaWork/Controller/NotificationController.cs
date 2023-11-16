using Application.BaseNotifications.Queries.GetCurrentAccountNotifications;
using Application.BaseNotifications.Queries.GetCurrentAccountNotificationsByTime;
using Application.BaseNotifications.Queries.Models;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebDiplomaWork.Controller;

public class NotificationController : ApiV1ControllerBase
{
    [HttpGet("by/current/account"), Authorize]
    public async Task<PaginatedList<BaseNotificationDto>> GetCurrentAccountNotifications(
        [FromQuery] GetCurrentAccountNotificationsQuery command)
    {
        return await Mediator.Send(command);
    }
    
    [HttpGet("by/time"), Authorize]
    public async Task<BaseNotificationDto[]> GetCurrentAccountNotificationsByTime(
        [FromQuery] GetCurrentAccountNotificationsByTimeQuery command)
    {
        return await Mediator.Send(command);
    }
}