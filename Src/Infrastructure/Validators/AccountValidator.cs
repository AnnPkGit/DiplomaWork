using App.Common.Interfaces;
using App.Common.Interfaces.Validators;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Validators;

public class AccountValidator : IAccountValidator
{
    private readonly IApplicationDbContext _dbContext;

    public AccountValidator(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsLoginUniqueAsync(
        string login)
    {
        // Проверка уникальности Login
        return await _dbContext.Accounts.AllAsync(a => a.Login != login);
    }

    public async Task<bool> IsAccountLimitReachedAsync(
        int userId,
        CancellationToken token)
    {
        var currentUser = await _dbContext.Users
            .Include(u => u.Accounts)
            .SingleOrDefaultAsync(u => u.Id == userId, token);
        
        if (currentUser == null)
            return false; //TODO: add ValidationException
        
        if (currentUser.MaxAccountsCount == currentUser.Accounts.Count)
            return false; //TODO: add ValidationException

        return true;
    }
}