using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.UpdateAccount;

[Authorize]
public record UpdateAccountCommand : IRequest
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public int? AvatarId { get; set; }
    public string? Bio { get; set; }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var entity = await _context.Accounts.FindAsync(
            new object?[] { request.Id }, token);
        if (entity == null)
            throw new NotFoundException(nameof(Account), request.Id);
        
        // Checking whether the account being deleted belongs to the user deleting it
        if (entity.Id != userId)
            throw new ForbiddenAccessException($"User ({userId}) tried to update account ({request.Id}).");
        
        if (request.AvatarId != null && !await _context.AvatarMediaItems.AnyAsync(item => item.Id == request.AvatarId, token))
        {
            throw new NotFoundException(nameof(AvatarMediaItem), request.AvatarId);
        }
        
        entity.Login = request.Login;
        entity.Name = request.Name;
        entity.BirthDate = request.BirthDate;
        entity.Bio = request.Bio;
        entity.AvatarId = request.AvatarId;

        await _context.SaveChangesAsync(token);
    }
}