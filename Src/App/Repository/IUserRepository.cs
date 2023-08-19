using Domain.Entity;

namespace App.Repository;

public interface IUserRepository
{
    IQueryable<User> GetAll();
    Task AddUserAsync(User user);
}
