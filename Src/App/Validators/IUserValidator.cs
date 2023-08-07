namespace App.Validators;

public interface IUserValidator
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsLoginUniqueAsync(string login);
    Task<bool> IsPasswordStrongAsync(string password);
}