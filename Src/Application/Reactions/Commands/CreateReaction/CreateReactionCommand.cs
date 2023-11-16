using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
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
        var toastWithContentId = request.ToastWithContentId;
        var toastWithContent = await _context.BaseToastsWithContent.FindAsync( new object?[]{ toastWithContentId } , cancellationToken);
        
        if (toastWithContent == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }

        if (await _context.Reactions.AnyAsync(r => 
                r.ToastWithContentId == toastWithContentId &&
                r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var createDate = _dateTime.Now;
        var newReaction = new Reaction(toastWithContentId, accountId, createDate);
        
        await _context.Reactions.AddAsync(newReaction, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && toastWithContent.AuthorId != accountId)
        {
            var newReactNotification = new ReactionNotification(toastWithContent.AuthorId, newReaction.Id, createDate);
            await _context.BaseNotifications.AddAsync(newReactNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}