using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Validators;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand : IRequest<int>
{
    public string Login { get; init; } = string.Empty;
    public string? Name { get; init; }
    public string? Avatar { get; init; } // TODO: Replace the folder type
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, int>
{
    private readonly IAccountValidator _validator;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateAccountCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IAccountValidator validator)
    {
        _context = context;
        _currentUserService = currentUserService;
        _validator = validator;
    }

    public async Task<int> Handle(CreateAccountCommand request, CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        if(userId == -1)
            throw new ValidationException("Something wrong");
        
        if (!await _validator.IsAccountLimitReachedAsync(userId, token))
            throw new ValidationException("Accounts limit exceeded");

        if (!await _validator.IsLoginUniqueAsync(request.Login))
            throw new ValidationException("This login is already taken.");
        
        var entity = new Account
        {
            Login = request.Login,
            Name = request.Name,
            Avatar = request.Avatar,
            CreateDt = DateTime.Now.ToUniversalTime()
        };
        
        var currentUser = await _context.Users.
            SingleOrDefaultAsync(u => u.Id == userId, token);
        entity.Owner = currentUser!;
        
        await _context.Accounts.AddAsync(entity, token);
        await _context.SaveChangesAsync(token);
        
        return entity.Id;
    }
}