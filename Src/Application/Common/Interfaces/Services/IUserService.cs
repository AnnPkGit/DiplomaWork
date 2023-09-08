using Application.Services;
using Domain.Common;
using Domain.Entity;

namespace Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<Result> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<Result<LoginResponse>> LoginUserAsync(LoginRequest user, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken); // temporary method
    Task<Result> DeleteUserAsync(CancellationToken cancellationToken);
    Task<Result> ChangePasswordAsync(string newPassword, CancellationToken cancellationToken);
    Task<Result> ChangeEmailAsync(string newEmail, CancellationToken cancellationToken);
    Task<Result> ChangePhoneAsync(string newPhone, CancellationToken cancellationToken);
}