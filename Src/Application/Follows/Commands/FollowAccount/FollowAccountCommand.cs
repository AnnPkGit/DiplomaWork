using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Commands.FollowAccount;

[Authorize]
public record FollowAccountCommand(int AccountId) : IRequest;

public class FollowAccountCommandHandler : IRequestHandler<FollowAccountCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly IMuteNotificationOptionsChecker _optionsChecker;

    public FollowAccountCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IDateTime dateTime,
        IMuteNotificationOptionsChecker optionsChecker)
    {
        _userService = userService;
        _context = context;
        _dateTime = dateTime;
        _optionsChecker = optionsChecker;
    }

    public async Task Handle(FollowAccountCommand request, CancellationToken cancellationToken)
    {
        var fromAccountId = _userService.Id;
        var followToId = request.AccountId;
        
        if (!await _context.Accounts.AnyAsync(a => a.Id == followToId, cancellationToken))
        {
            throw new NotFoundException(nameof(Account), followToId);
        }
        
        if (await _context.Follows.AnyAsync(f => f.FollowFromId == fromAccountId && f.FollowToId == followToId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var followTime = _dateTime.Now;
        var newFollow = new Follow
        {
            FollowFromId = fromAccountId,
            FollowToId = followToId,
            DateOfFollow = followTime
        };

        await _context.Follows.AddAsync(newFollow, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && await _optionsChecker.CheckMuteOptions(fromAccountId, followToId, cancellationToken))
        {
            var newFollowerNotification = new FollowerNotification(followToId, fromAccountId, followTime);
            await _context.BaseNotifications.AddAsync(newFollowerNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}