using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands.CreateAccount;

[Authorize]
public record CreateAccountCommand : IRequest<int>
{
    public int UserId { get; set; }
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
        var userId = request.UserId;
        if (userId == UserDefaultValues.Id)
            throw new NotFoundException();
        
        var entity = new Account
        {
            Id = userId,
            Login = request.Login,
            Name = request.Name,
            Avatar = request.Avatar
        };

        await _context.Accounts.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        
        return entity.Id;
    }
}