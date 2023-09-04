using Domain.Common;
using Domain.Entity;

namespace App.Common.Interfaces.Services;

public sealed record UpdateAccountModel
{
    public string Login { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
}
public interface IAccountService
{
    Task<Result<int>> CreateAccountAsync(Account account, CancellationToken cancellationToken);
    Task<IEnumerable<Account>> GetAllAccounts(CancellationToken cancellationToken);
    Task<Result> DeleteAccountAsync(int id, CancellationToken cancellationToken);
    Task<Result> FullyUpdateAccountAsync(int id, UpdateAccountModel model, CancellationToken cancellationToken);
    Task<Account?> UpdateAccountAsync(int id, UpdateAccountModel model, CancellationToken cancellationToken);
    Task<Account> GetAccountByIdAsync(int id, CancellationToken cancellationToken);
    Task<Account> GetAccountByLoginAsync(string login, CancellationToken cancellationToken);
}