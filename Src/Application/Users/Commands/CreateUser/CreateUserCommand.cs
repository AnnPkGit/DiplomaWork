using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password) : IRequest;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUserValidator _validator;
    private readonly IHasher _hasher;
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(
        IUserValidator validator,
        IHasher hasher,
        IApplicationDbContext context)
    {
        _validator = validator;
        _hasher = hasher;
        _context = context;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken token)
    {
        if (! await _validator.IsPasswordStrongAsync(request.Password))
            throw new ValidationException("The password must contain uppercase and lowercase letters, digits, special characters, and be at least 8 characters long.");

        if (!await _validator.IsEmailUniqueAsync(request.Email))
            throw new ValidationException("This email is already taken.");
        
        var entity = new User
        {
            Email = request.Email,
            Password = request.Password,
            PasswordSalt = _hasher.GenerateSalt()
        };

        entity.Password = _hasher.HashPassword(entity.Password, entity.PasswordSalt);
        entity.RegistrationDt = DateTime.Now.ToUniversalTime();
        entity.MaxAccountsCount = 1;

        
        await _context.Users.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }
}