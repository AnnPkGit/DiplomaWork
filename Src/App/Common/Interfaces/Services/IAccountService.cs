using Domain.Common;
using Domain.Entity;

namespace App.Common.Interfaces.Services;

public interface IAccountService
{
    Task<Result<int>> CreateAccountAsync(Account account, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllAccounts(CancellationToken cancellationToken);
}