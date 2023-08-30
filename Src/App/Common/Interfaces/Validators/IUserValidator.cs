namespace App.Common.Interfaces.Validators;

public interface IUserValidator
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsPasswordStrongAsync(string password);
    Task<bool> IsNewPasswordUnequalAsync(int id, string password, CancellationToken cancellationToken);
    Task<bool> IsPhoneUniqueAsync(string phone, CancellationToken cancellationToken);
}