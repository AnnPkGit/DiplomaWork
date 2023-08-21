namespace App.Common.Interfaces.Validators;

public interface IUserValidator
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsPasswordStrongAsync(string password);
}