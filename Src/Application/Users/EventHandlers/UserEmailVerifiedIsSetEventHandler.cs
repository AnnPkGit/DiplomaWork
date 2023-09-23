using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events;
using MediatR;

namespace Application.Users.EventHandlers;

public class UserEmailVerifiedIsSetEventHandler : INotificationHandler<UserEmailVerifiedIsSetEvent>
{
    private readonly ICurrentUserService _currentUserService;
    public UserEmailVerifiedIsSetEventHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task Handle(UserEmailVerifiedIsSetEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.Item;
        await _currentUserService.SetEmailVerifiedAsync(user.EmailVerified, cancellationToken: cancellationToken);
        if (user.EmailVerified)
        {
            if (user.Roles.Any(role => !role.Equals(Role.Verified)))
            {
                user.Roles.Add(Role.Verified);
                await _currentUserService.SetRolesAsync(user.Roles, cancellationToken: cancellationToken);
            }
        }
        else
        {
            if (user.Roles.Any(role => role.Equals(Role.Verified)))
            {
                user.Roles.Remove(Role.Verified);
                await _currentUserService.SetRolesAsync(user.Roles, cancellationToken: cancellationToken);
            }
        }
    }
}