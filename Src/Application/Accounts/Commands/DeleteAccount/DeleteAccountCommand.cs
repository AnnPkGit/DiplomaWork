using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(int Id) : IRequest;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAccountCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var entity = await _context.Accounts.FindAsync(
            new object?[] { request.Id }, token);
        if (entity == null)
            throw new NotFoundException(nameof(Account), request.Id);
        
        // Checking whether the account being deleted belongs to the user deleting it
        if (entity.OwnerId != userId)
            throw new ForbiddenAccessException($"User ({userId}) tried to delete account ({request.Id}).");

        entity.DeactivationDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(token);
    }
}