using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;

namespace Application.Users.Commands.UpdateUserPassword;

public record UpdateUserPasswordCommand(string NewPassword) : IRequest;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserValidator _validator;
    private readonly IHasher _hasher;

    public UpdateUserPasswordCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IUserValidator validator,
        IHasher hasher)
    {
        _context = context;
        _currentUserService = currentUserService;
        _validator = validator;
        _hasher = hasher;
    }

    public async Task Handle(UpdateUserPasswordCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        var user = await _context.Users.FindAsync(new object?[] { userId }, token);
        
        if (user == null)
            throw new NotFoundException(nameof(User), userId);
        
        if (! await _validator.IsPasswordStrongAsync(request.NewPassword))
            throw new ValidationException("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");
        
        if (! await _validator.IsNewPasswordUnequalAsync(userId, request.NewPassword, token))
            throw new ValidationException("The new password must be different from the old one");

        var newSalt = _hasher.GenerateSalt();
        var newPassHash = _hasher.HashPassword(request.NewPassword, newSalt);

        user.PasswordSalt = newSalt;
        user.Password = newPassHash;

        await _context.SaveChangesAsync(token);
    }
}