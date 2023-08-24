namespace App.Common.Interfaces.Validators;

public interface IAccountValidator
{
    Task<bool> IsLoginUniqueAsync(string login);
    Task<bool> IsAccountLimitReachedAsync(int userId, CancellationToken cancellationToken);
}