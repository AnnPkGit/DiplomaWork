using Application.Common.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand : IRequest<int>
{
    public string Login { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? Avatar { get; init; } // TODO: Replace the folder type
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAccountCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        
        var entity = new Account
        {
            Login = request.Login,
            Name = request.Name,
            Avatar = request.Avatar,
            CreateDt = DateTime.Now.ToUniversalTime(),
            OwnerId = userId
        };

        await _context.Accounts.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        
        return entity.Id;
    }
}