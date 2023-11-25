using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Commands.UnfollowAccount;

[Authorize]
public record UnfollowAccountCommand(int FollowingId) : IRequest;

public class UnfollowCommandHandler : IRequestHandler<UnfollowAccountCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    
    public UnfollowCommandHandler(ICurrentUserService userService, IApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }
    
    public async Task Handle(UnfollowAccountCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var followingId = request.FollowingId;
        
        var follow = await _context.Follows
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FollowFromId == accountId && f.FollowToId == followingId, cancellationToken);
        
        if (follow == null)
        {
            throw new NotFoundException(nameof(Follow));
        }
        
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync(cancellationToken);
    }
}