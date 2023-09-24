using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands.UpdateAccountDetail;

[Authorize]
public record UpdateAccountDetailCommand : IRequest
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountDetailCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateAccountDetailCommand request, CancellationToken token)
    {
        var userId = _currentUserService.Id;
        var entity = await _context.Accounts.FindAsync(
            new object?[] { request.Id }, token);
        if (entity == null)
            throw new NotFoundException(nameof(Account), request.Id);
        
        // Checking whether the account being deleted belongs to the user deleting it
        if (entity.Id != userId)
            throw new ForbiddenAccessException($"User ({userId}) tried to update account ({request.Id}).");

        var newLogin = request.Login;
        if (!string.IsNullOrEmpty(newLogin))
            entity.Login = newLogin;
        
        var newName = request.Name;
        if (newName != null)
            entity.Name = newName;
        
        var newBirthDate = request.BirthDate;
        if (newBirthDate != default)
            entity.BirthDate = newBirthDate;
        
        var newBio = entity.Bio;
        if (newName != null)
            entity.Bio = newBio;

        var newAvatar = request.Avatar;
        if (!string.IsNullOrEmpty(newAvatar))
            entity.Avatar = newAvatar;
        
        await _context.SaveChangesAsync(token);
    }
}