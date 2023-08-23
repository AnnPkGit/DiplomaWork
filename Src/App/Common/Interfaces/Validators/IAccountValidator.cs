namespace App.Common.Interfaces;

public interface IAccountValidator
{
    Task<bool> IsLoginUniqueAsync(string login);
    Task<bool> IsAccountLimitReachedAsync(Guid userId, CancellationToken cancellationToken);
}