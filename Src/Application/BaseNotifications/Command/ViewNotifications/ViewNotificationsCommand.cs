using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseNotifications.Command.ViewNotifications;

[Authorize]
public record ViewNotificationsCommand(params int[] BaseNotificationIds) : IRequest;

public class ViewNotificationsCommandHandler : IRequestHandler<ViewNotificationsCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public ViewNotificationsCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IDateTime dateTime)
    {
        _userService = userService;
        _context = context;
        _dateTime = dateTime;
    }

    public async Task Handle(ViewNotificationsCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var notificationIds = request.BaseNotificationIds;

        if (!notificationIds.Any())
            return;
        
        var notifications = await _context.BaseNotifications
            .Where(notification => notification.ToAccountId == accountId && notificationIds.Contains(notification.Id))
            .ToArrayAsync(cancellationToken);

        var viewDataTime = _dateTime.Now;
        
        if (notifications.Any())
        {
            foreach (var notification in notifications)
            {
                notification.Viewed = viewDataTime;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}