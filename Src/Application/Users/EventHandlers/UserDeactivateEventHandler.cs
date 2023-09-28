using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserDeactivateEventHandler : INotificationHandler<UserDeactivateEvent>
{
    private readonly IDateTime _dateTime;

    public UserDeactivateEventHandler(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public Task Handle(UserDeactivateEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.Item;
        if (user.Account is not null)
        {
            user.Account.Deactivated = _dateTime.Now;
        }
        return Task.CompletedTask;
    }
}