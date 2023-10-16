using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Commands.DeleteToast;

[Authorize]
public record DeleteToastCommand(int ToastId) : IRequest;

public class DeleteToastCommandHandler : IRequestHandler<DeleteToastCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public DeleteToastCommandHandler(
        IApplicationDbContext context,
        IDateTime dateTime,
        ICurrentUserService userService)
    {
        _context = context;
        _dateTime = dateTime;
        _userService = userService;
    }

    public async Task Handle(DeleteToastCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var toast = await _context.Toasts.SingleOrDefaultAsync(t => t.Id == request.ToastId && t.AuthorId == userId, cancellationToken);
        
        if (toast == null)
        {
            throw new NotFoundException(nameof(toast), request.ToastId);
        }
        
        toast.Deactivated = _dateTime.Now;
        toast.DeactivatedById = userId;

        await _context.SaveChangesAsync(cancellationToken);
    }
}