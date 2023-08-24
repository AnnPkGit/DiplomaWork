using App.Common.Interfaces;
using App.Common.Interfaces.Services;
using App.Common.Interfaces.Validators;
using Domain.Common;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Services;

public sealed class AccountService : IAccountService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAccountValidator _validator;
    private readonly ILogger<AccountService> _logger;
    private readonly ICurrentUserService _currentUserService;

    public AccountService(
        IApplicationDbContext dbContext,
        IAccountValidator validator,
        ILogger<AccountService> logger, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> CreateAccountAsync(Account account,
        CancellationToken token)
    {
        var userId = _currentUserService.UserId;
        if(userId == -1)
            return Result<int>.Failed("Something wrong");
        if (!await _validator.IsLoginUniqueAsync(account.Login))
            return Result<int>.Failed("This login is already taken.");
        
        if (!await _validator.IsAccountLimitReachedAsync(userId, token))
            return Result<int>.Failed("Accounts limit exceeded");
        
        account.CreateDt = DateTime.Now.ToUniversalTime();
        var currentUser = await _dbContext.Users.
            SingleOrDefaultAsync(u => u.Id == userId, token);
        account.Owner = currentUser!;
            
        try
        {
            await _dbContext.Accounts.AddAsync(account, token);
            await _dbContext.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<int>.Failed("Something went wrong " + ex.Message);
        }
        
        return Result<int>.Successful(account.Id);
    }

    public async Task<IEnumerable<Account>> GetAllAccounts(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts.ToListAsync(cancellationToken);
    }
}