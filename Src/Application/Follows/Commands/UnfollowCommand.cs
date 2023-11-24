using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Follows.Commands
{
    [Authorize]
    public record UnfollowCommand(int FollowingId) : IRequest<Unit>;

    public class UnfollowCommandHandler : IRequestHandler<UnfollowCommand, Unit>
    {
        private readonly ICurrentUserService _userService;
        private readonly IApplicationDbContext _context;

        public UnfollowCommandHandler(ICurrentUserService userService, IApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<Unit> Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {
            var followerId = _userService.Id;
            var followingId = request.FollowingId;

            var follow = await _context.Follows
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.AccountId == followerId && f.ToAccountId == followingId, cancellationToken);


            if (follow != null)
            {
                _context.Follows.Remove(follow);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}