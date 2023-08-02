using Domain.Entity;

namespace App.Repository;

public interface IUserRepository
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<bool> IsLoginUniqueAsync(string login);
    Task<bool> IsPasswordStrongAsync(string password);
    Task AddUserAsync(User user);
}