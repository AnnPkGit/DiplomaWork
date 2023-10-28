using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Commands.CreateReaction;

[Authorize]
public record CreateReactionCommand(int ToastWithContentId) : IRequest;

public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IDateTime _dateTime;

    public CreateReactionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IDateTime dateTime)
    {
        _context = context;
        _userService = userService;
        _dateTime = dateTime;
    }

    public async Task Handle(CreateReactionCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == request.ToastWithContentId, cancellationToken))
        {
            throw new NotFoundException(nameof(Toast), request.ToastWithContentId);
        }

        if (await _context.Reactions.AnyAsync(r => 
                r.ToastWithContentId == request.ToastWithContentId &&
                r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var newReaction = new Reaction(request.ToastWithContentId, accountId, _dateTime.Now);

        await _context.Reactions.AddAsync(newReaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}