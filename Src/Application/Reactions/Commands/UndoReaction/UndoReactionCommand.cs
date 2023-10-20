using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Commands.UndoReaction;

[Authorize]
public class UndoReactionCommand : IRequest
{
    public int ToastId { get; set; }
}

public class UndoReactionCommandHandler : IRequestHandler<UndoReactionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;

    public UndoReactionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task Handle(UndoReactionCommand request, CancellationToken token)
    {
        var accountId = _userService.Id;

        var reaction = await _context.Reactions
            .SingleOrDefaultAsync(t => t.AuthorId == accountId && t.ToastId == request.ToastId, token);
        
        if (reaction == null)
        {
            throw new NotFoundException();
        }
        
        _context.Reactions.Remove(reaction);

        await _context.SaveChangesAsync(token);
    }
}