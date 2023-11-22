using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ReToasts.Commands.DeleteReToastByToastId;

[Authorize]
public record DeleteReToastByToastIdCommand(int ToastWithContentId) : IRequest;

public class DeleteReToastByToastIdCommandHandler : IRequestHandler<DeleteReToastByToastIdCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public DeleteReToastByToastIdCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context,
        IDateTime dateTime)
    {
        _userService = userService;
        _context = context;
        _dateTime = dateTime;
    }

    public async Task Handle(DeleteReToastByToastIdCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        var toastWithContentId = request.ToastWithContentId;
        var toastWithContent = await _context.ReToasts
            .SingleOrDefaultAsync(reToast =>
                reToast.AuthorId == accountId &&
                reToast.ToastWithContentId == toastWithContentId,
                cancellationToken);

        if (toastWithContent == null)
        {
            throw new NotFoundException(nameof(BaseToastWithContent), toastWithContentId);
        }

        toastWithContent.DeactivatedById = accountId;
        toastWithContent.Deactivated = _dateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
    }
}