using Application.Auth.Commands.SendVerifyMsgByEmail;
using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserEmailIsSetEventHandler : INotificationHandler<UserEmailIsSetEvent>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISender _mediator;
    public UserEmailIsSetEventHandler(
        ISender mediator,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UserEmailIsSetEvent notification, CancellationToken token)
    {
        var user = notification.Item;
        
        await _currentUserService.SetEmailAsync(user.Email, cancellationToken: token);
        await _mediator.Send(new SendVerifyMsgByEmailCommand(), token);
    }
}