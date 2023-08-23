using App.Services;
using Domain.Common;
using Domain.Entity;

namespace App.Common.Interfaces.Services;

public interface IUserService
{
    Task<Result> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> LoginUserAsync(LoginRequest user, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
}