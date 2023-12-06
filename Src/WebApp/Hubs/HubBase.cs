using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class HubBase : Hub
{
    private ISender? _mediator;

    protected ISender? Mediator => _mediator ??= Context.GetHttpContext()?.RequestServices.GetRequiredService<ISender>();
}