using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Email, string Password) : BaseCreateUserModel(Email, Password), IRequest<int>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IHasher _hasher;
    private readonly IApplicationDbContext _context;
    private readonly IEmailConfirmationSender _emailSender;

    public CreateUserCommandHandler(
        IHasher hasher,
        IApplicationDbContext context,
        IEmailConfirmationSender emailSender)
    {
        _hasher = hasher;
        _context = context;
        _emailSender = emailSender;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken token)
    {
        var newPassSalt = _hasher.GenerateSalt();
        var hashPassword = _hasher.HashPassword(request.Password, newPassSalt);
        
        var entity = new User(request.Email, hashPassword, newPassSalt);
        
        await _context.Users.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        await _emailSender.SendAsync(new EmailConfirmRequest(entity.Id, entity.Email), token);
        return entity.Id;
    }
}