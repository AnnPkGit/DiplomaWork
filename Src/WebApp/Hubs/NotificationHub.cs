using Application.BaseNotifications.Command.ViewNotifications;
using Application.BaseNotifications.Queries.GetAllCurrentAccountNotifications;
using Application.BaseNotifications.Queries.GetCurrentAccountNotificationsByTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

[Authorize]
public class NotificationHub : HubBase
{
    public async Task ViewNotifications(ViewNotificationsCommand command)
    {
        if (Mediator is not null)
        {
            await Mediator.Send(command);
            await Clients.Caller.SendAsync("Receive", "OK");
            return;
        }
        await Clients.Caller.SendAsync("Receive", null);
    }
    public async Task Sync(GetCurrentAccountNotificationsByTimeQuery query)
    {
        if (Mediator is not null)
        {
            var result = await Mediator.Send(query);
            await Clients.Caller.SendAsync("Receive", result);
            return;
        }
        await Clients.Caller.SendAsync("Receive", null);
    }

    public override async Task OnConnectedAsync()
    {
        if (Mediator is not null)
        {
            var result = await Mediator.Send(new GetAllCurrentAccountNotificationsQuery());
            await Clients.Caller.SendAsync("Receive", result);
        }
        await base.OnConnectedAsync();
    }
}