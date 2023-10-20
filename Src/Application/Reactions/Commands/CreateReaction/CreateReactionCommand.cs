using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reactions.Commands.CreateReaction;

[Authorize]
public record CreateReactionCommand(int ToastId) : IRequest;

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
        if (!await _context.Toasts.AnyAsync(t => t.Id == request.ToastId, cancellationToken))
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }

        if (await _context.Reactions.AnyAsync(r => r.ToastId == request.ToastId && r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var newReaction = new Reaction(request.ToastId, accountId, _dateTime.Now);

        await _context.Reactions.AddAsync(newReaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}