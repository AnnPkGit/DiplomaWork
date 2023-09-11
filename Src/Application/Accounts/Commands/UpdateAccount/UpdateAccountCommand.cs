using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;

namespace Application.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand : IRequest
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IAccountValidator _validator;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAccountCommandHandler(
        IAccountValidator validator,
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _validator = validator;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken token)
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

        if (!await _validator.IsLoginUniqueAsync(newLogin))
            throw new ValidationException($"Login ({newLogin}) is already taken");

        entity.Login = newLogin;
        entity.Name = request.Name;
        entity.BirthDate = request.BirthDate;
        entity.Bio = request.Bio;
        entity.Avatar = request.Avatar;

        await _context.SaveChangesAsync(token);
    }
}