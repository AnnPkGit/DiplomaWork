using App.Common.Interfaces;
using App.Common.Interfaces.Services;
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

    public async Task<Result<string>> CreateAccountAsync(
        Account account,
        CancellationToken token)
    {
        if(_currentUserService.UserId == null)
            return Result<string>.Failed("Something wrong");
        if (!await _validator.IsLoginUniqueAsync(account.Login))
            return Result<string>.Failed("This login is already taken.");
        
        var userId = Guid.Parse(_currentUserService.UserId);
        if (!await _validator.IsAccountLimitReachedAsync(userId, token))
            return Result<string>.Failed("Accounts limit exceeded");
        
        account.Id = Guid.NewGuid();
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
            return Result<string>.Failed("Something went wrong " + ex.Message);
        }
        return Result<string>.Successful(account.Id.ToString());
    }

    public async Task<IEnumerable<Account>> GetAllAccounts(
        CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts.ToListAsync(cancellationToken);
    }
}