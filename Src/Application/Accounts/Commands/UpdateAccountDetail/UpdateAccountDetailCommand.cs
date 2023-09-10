using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;

namespace Application.Accounts.Commands.UpdateAccountDetail;

public record UpdateAccountDetailCommand : IRequest
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}

public class UpdateAccountDetailCommandHandler : IRequestHandler<UpdateAccountDetailCommand>
{
    private readonly IAccountValidator _validator;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountDetailCommandHandler(
        IAccountValidator validator,
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _validator = validator;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateAccountDetailCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var entity = await _context.Accounts.FindAsync(
            new object?[] { request.Id }, token);
        if (entity == null)
            throw new NotFoundException(nameof(Account), request.Id);
        
        // Checking whether the account being deleted belongs to the user deleting it
        if (entity.OwnerId != userId)
            throw new ForbiddenAccessException($"User ({userId}) tried to update account ({request.Id}).");

        var newLogin = request.Login;
        if (!string.IsNullOrEmpty(newLogin))
        {
            if (!await _validator.IsLoginUniqueAsync(newLogin))
                throw new ValidationException($"Login ({newLogin}) is already taken");
            
            entity.Login = newLogin;
        }
        
        var newName = request.Name;
        if (!string.IsNullOrEmpty(newName))
        {
            entity.Name = newName;
        }
        
        var newBirthDate = request.BirthDate;
        if (newBirthDate != default)
        {
            entity.BirthDate = newBirthDate;
        }
        
        var newBio = entity.Bio;
        if (!string.IsNullOrEmpty(newBio))
        {
            entity.Bio = newBio;
        }

        var newAvatar = request.Avatar;
        if (!string.IsNullOrEmpty(newAvatar))
        {
            entity.Avatar = newAvatar;
        }
        
        await _context.SaveChangesAsync(token);
    }
}