using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserCreateEventHandler : INotificationHandler<UserCreateEvent>
{
    private readonly IEmailConfirmationSender _emailSender;
    
    public UserCreateEventHandler(IEmailConfirmationSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task Handle(UserCreateEvent notification, CancellationToken token)
    {
        var user = notification.Item;
        
        await _emailSender.SendAsync(new EmailConfirmRequest(user.Id, user.Email), token);
    }
}