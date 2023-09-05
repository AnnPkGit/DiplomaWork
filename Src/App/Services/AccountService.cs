using App.Common.Exceptions;
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
        ILogger<AccountService> logger,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> CreateAccountAsync(
        Account account,
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

    public async Task<IEnumerable<Account>> GetAllAccounts(CancellationToken token)
    {
        return await _dbContext.Accounts
            .ToListAsync(token);
    }

    public async Task<Result> DeleteAccountAsync(
        int id,
        CancellationToken token)
    {
        var account = await GetByIdAsync(id, token);
        
        var userId = _currentUserService.UserId;
        // Checking whether the account being deleted belongs to the user deleting it
        if (account.OwnerId != userId)
            throw new ForbiddenAccessException($"User \"{userId}\" tried to delete account \"{id}\".");

        account.DeactivationDate = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(token);
        return Result.Successful();
    }

    public async Task<Result> FullyUpdateAccountAsync(
        int id,
        UpdateAccountModel model,
        CancellationToken token)
    {
        var account = await GetByIdAsync(id, token);
        
        var userId = _currentUserService.UserId;
        // Checking whether the account being changed belongs to the user deleting it
        if (account.OwnerId != userId)
            throw new ForbiddenAccessException($"User \"{userId}\" tried to changed account \"{id}\".");
        
        var newLogin = model.Login;

        if (!await _validator.IsLoginUniqueAsync(newLogin))
            throw new ValidationException($"Login \"{newLogin}\" is already taken");

        account.Login = newLogin;
        account.Name = model.Name;
        account.BirthDate = model.BirthDate;
        account.Bio = model.Bio;
        account.Avatar = model.Avatar;

        await _dbContext.SaveChangesAsync(token);

        return Result.Successful();
    }

    public async Task<Account?> UpdateAccountAsync(
        int id,
        UpdateAccountModel model,
        CancellationToken token)
    {
        var account = await GetByIdAsync(id, token);
        
        var userId = _currentUserService.UserId;
        // Checking whether the account being changed belongs to the user deleting it
        if (account.OwnerId != userId)
            throw new ForbiddenAccessException($"User \"{userId}\" tried to changed account \"{id}\".");
        
        var newLogin = model.Login;
        if (!string.IsNullOrEmpty(newLogin))
        {
            if (!await _validator.IsLoginUniqueAsync(newLogin))
                throw new ValidationException($"Login \"{newLogin}\" is already taken");
            
            account.Login = newLogin;
        }
        
        var newName = model.Name;
        if (!string.IsNullOrEmpty(newName))
        {
            account.Name = newName;
        }
        
        var newBirthDate = model.BirthDate;
        if (newBirthDate != default)
        {
            account.BirthDate = newBirthDate;
        }
        
        var newBio = account.Bio;
        if (!string.IsNullOrEmpty(newBio))
        {
            account.Bio = newBio;
        }

        var newAvatar = model.Avatar;
        if (!string.IsNullOrEmpty(newAvatar))
        {
            account.Avatar = newAvatar;
        }

        var res = await _dbContext.SaveChangesAsync(token);
        return res == 0 ? null : account;
    }

    public async Task<Account> GetAccountByIdAsync(int id, CancellationToken token)
    {
        return await GetByIdAsync(id, token);
    }

    public async Task<Account> GetAccountByLoginAsync(string login, CancellationToken token)
    {
        var account = await _dbContext.Accounts
            .SingleOrDefaultAsync(a => a.Login == login, token);
        
        if (account == null)
            throw new NotFoundException("Account", login);

        return account;
    }

    private async Task<Account> GetByIdAsync(int id, CancellationToken token)
    {
        var account = await _dbContext.Accounts.FindAsync(
            new object?[] { id },
            token);
        
        if (account == null)
            throw new NotFoundException("Account", id);

        return account;
    }
}