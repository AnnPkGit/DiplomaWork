using Application.Accounts.Commands.DeleteAccount;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand : IRequest;

public class DeleteUserCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);
        if (user == null)
            throw new NotFoundException(nameof(User), userId);
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(token);
    }
}