using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ReToasts.Commands.CreateReToast;

[Authorize]
public record CreateReToastCommand(int ToastWithContentId) : IRequest;

public class CreateReToastCommandHandler : IRequestHandler<CreateReToastCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;

    public CreateReToastCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task Handle(CreateReToastCommand request, CancellationToken cancellationToken)
    {
        var accountId = _userService.Id;
        if (!await _context.BaseToastsWithContent.AnyAsync(t => t.Id == request.ToastWithContentId, cancellationToken))
        {
            throw new NotFoundException(nameof(BaseToastWithContent), request.ToastWithContentId);
        }

        if (await _context.ReToasts.AnyAsync(r =>
                r.ToastWithContentId == request.ToastWithContentId &&
                r.AuthorId == accountId, cancellationToken))
        {
            throw new ForbiddenAccessException();
        }

        var newReToast = new ReToast(accountId, request.ToastWithContentId);
        
        await _context.ReToasts.AddAsync(newReToast, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}