using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Security;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands.CreateAccount;

[Authorize]
public class CreateAccountCommand : BaseCreateAccountModel, IRequest<int>
{
    public int UserId { get; set; }
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
        var userId = _currentUserService.Id;
        if (userId == UserDefaultValues.Id)
            throw new NotFoundException();
        
        var entity = new Account
        {
            Id = userId,
            Login = request.Login,
            Name = request.Name
        };

        await _context.Accounts.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        
        return entity.Id;
    }
}