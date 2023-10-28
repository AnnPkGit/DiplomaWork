using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.BaseToasts.Commands.DeleteToast;

[Authorize]
public record DeleteBaseToastCommand(int BaseToastId) : IRequest;

public class DeleteToastCommandHandler : IRequestHandler<DeleteBaseToastCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _userService;
    private readonly IDateTime _dateTime;

    public DeleteToastCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService userService,
        IDateTime dateTime)
    {
        _context = context;
        _userService = userService;
        _dateTime = dateTime;
    }

    public async Task Handle(DeleteBaseToastCommand request, CancellationToken token)
    {
        var userId = _userService.Id;
        var toast = await _context.BaseToasts
            .FirstOrDefaultAsync(bt => bt.Id == request.BaseToastId && bt.AuthorId == userId, token);

        if (toast == null)
        {
            throw new NotFoundException(nameof(BaseToast), request.BaseToastId);
        }
        
        toast.Deactivated = _dateTime.Now;
        toast.DeactivatedById = userId;

        await _context.SaveChangesAsync(token);
    }
}