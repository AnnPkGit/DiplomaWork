using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace WebDiplomaWork.Hubs;

public class HubBase : Hub
{
    private ISender? _mediator;

    protected ISender? Mediator => _mediator ??= Context.GetHttpContext()?.RequestServices.GetRequiredService<ISender>();
}