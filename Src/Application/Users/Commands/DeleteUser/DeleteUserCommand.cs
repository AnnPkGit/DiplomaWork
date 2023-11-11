using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.DeleteUser;

[Authorize]
public record DeleteUserCommand : IRequest;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    public DeleteUserCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IDateTime dateTime)
    {
        _context = context;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);
        if (user == null)
            throw new NotFoundException(nameof(User), userId);

        user.Deactivated = _dateTime.Now;
        await _currentUserService.Clear(token);
        await _context.SaveChangesAsync(token);
    }
}