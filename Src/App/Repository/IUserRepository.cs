using Domain.Entity;

namespace App.Repository;

public interface IUserRepository
{
    Task AddUserAsync(User user);
}
