using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password) : IRequest;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IHasher _hasher;
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(
        IHasher hasher,
        IApplicationDbContext context)
    {
        _hasher = hasher;
        _context = context;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken token)
    {
        var entity = new User
        {
            Email = request.Email,
            Password = request.Password,
            PasswordSalt = _hasher.GenerateSalt()
        };

        entity.Password = _hasher.HashPassword(entity.Password, entity.PasswordSalt);
        entity.MaxAccountsCount = 1;

        
        await _context.Users.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }
}