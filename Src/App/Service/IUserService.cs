using App.Users.Login;
using Domain.Common;
using Domain.Entity;

namespace App.Service;

public interface IUserService
{
    Task<Result> CreateUserAsync(User user);
    Task<Result<LoginResponse>> LoginUserAsync(LoginRequest user);
    Task<IEnumerable<User>> GetAllUsersAsync();
}