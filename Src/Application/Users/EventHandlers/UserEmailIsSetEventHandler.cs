using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserEmailIsSetEventHandler : INotificationHandler<UserEmailIsSetEvent>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailConfirmationSender _emailSender;
    public UserEmailIsSetEventHandler(
        ICurrentUserService currentUserService,
        IEmailConfirmationSender emailSender)
    {
        _currentUserService = currentUserService;
        _emailSender = emailSender;
    }

    public async Task Handle(UserEmailIsSetEvent notification, CancellationToken token)
    {
        var user = notification.Item;
        
        await _currentUserService.SetEmailAsync(user.Email, cancellationToken: token);
        if (!user.EmailVerified)
        {
            await _emailSender.SendAsync(new EmailConfirmRequest(user.Id, user.Email), token);
        }
    }
}