using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Toasts.Commands.UndoReToast;

[Authorize]
public record UndoReToastCommand(int ToastId) : IRequest;

public class UndoReToastCommandHandler : IRequestHandler<UndoReToastCommand>
{
    private readonly ICurrentUserService _userService;
    private readonly IApplicationDbContext _context;

    public UndoReToastCommandHandler(
        ICurrentUserService userService,
        IApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task Handle(UndoReToastCommand request, CancellationToken cancellationToken)
    {
        var userId = _userService.Id;
        var reToast = await _context.ReToasts
            .SingleOrDefaultAsync(t => t.ToastId == request.ToastId && t.AccountId == userId , cancellationToken);
        
        if (reToast == null)
        {
            throw new NotFoundException(nameof(Toast), request.ToastId);
        }
        
        _context.ReToasts.Remove(reToast);
        await _context.SaveChangesAsync(cancellationToken);
    }
}