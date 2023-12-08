using Application.Accounts.Commands.CreateAccount;
using Application.Common.Interfaces;
using Application.Users.Commands.CreateUser;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.RegistrateUserWithAccount;

public class RegistrateUserAndAccountCommand : IRequest
{
    public CreateUserModel User { get; set; } = null!;
    public CreateAccountModel Account { get; set; } = null!;
}

public class RegistrateUserAndAccountCommandHandler : IRequestHandler<RegistrateUserAndAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailConfirmationSender _emailSender;
    private readonly IHasher _hasher;

    public RegistrateUserAndAccountCommandHandler(
        IApplicationDbContext context,
        IEmailConfirmationSender emailSender,
        IHasher hasher)
    {
        _context = context;
        _emailSender = emailSender;
        _hasher = hasher;
    }

    public async Task Handle(RegistrateUserAndAccountCommand request, CancellationToken token)
    {
        var newUserId = await CreateNewUser(request.User, token);
        await CreateNewAccount(request.Account, newUserId, token);
    }

    private async Task<int> CreateNewUser(CreateUserModel model, CancellationToken token)
    {
        var newPassSalt = _hasher.GenerateSalt();
        var hashPassword = _hasher.HashPassword(model.Password, newPassSalt);
        
        var entity = new User(model.Email, hashPassword, newPassSalt);
        
        await _context.Users.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        await _emailSender.SendAsync(new EmailConfirmRequest(entity.Id, entity.Email), token);
        return entity.Id;
    }
    
    private async Task CreateNewAccount(CreateAccountModel model, int userId, CancellationToken token)
    {
        var entity = new Account
        {
            Id = userId,
            Login = model.Login,
            Name = model.Name
        };

        await _context.Accounts.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
    }
}

public record CreateUserModel(string Email, string Password) : BaseCreateUserModel(Email, Password);

public class CreateAccountModel : BaseCreateAccountModel { }