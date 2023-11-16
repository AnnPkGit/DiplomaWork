using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ReToasts.Commands.CreateReToast;

[Authorize]
public record CreateReToastCommand(int ToastWithContentId) : IRequest;

public class CreateReToastCommandHandler : IRequestHandler<CreateReToastCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IDateTime _dateTime;

    public CreateReToastCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IDateTime dateTime)
    {
        _context = context;
        _userService = userService;
        _dateTime = dateTime;
    }

    public async Task Handle(CreateReToastCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var toastWithContentId = request.ToastWithContentId;
        var toastWithContent = await _context.BaseToastsWithContent.FindAsync( new object?[]{ toastWithContentId } , cancellationToken);

        if (toastWithContent == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }

        if (await _context.ReToasts.AnyAsync(r =>
                r.ToastWithContentId == toastWithContentId &&
                r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var createDate = _dateTime.Now;
        var newReToast = new ReToast(accountId, toastWithContentId);
        
        await _context.ReToasts.AddAsync(newReToast, cancellationToken);
        var res = await _context.SaveChangesAsync(cancellationToken);
        if (res != 0 && toastWithContent.AuthorId != accountId)
        {
            var newReToastNotification = new ReToastNotification(toastWithContent.AuthorId, newReToast.Id, createDate);
            await _context.BaseNotifications.AddAsync(newReToastNotification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}