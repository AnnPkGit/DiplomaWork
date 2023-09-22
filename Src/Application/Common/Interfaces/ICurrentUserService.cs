using Application.Common.Constants;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    int Id { get; }
    string Email { get; }
    public bool? EmailVerified { get; }
    HashSet<string> Roles { get; }
    HashSet<string> Permission { get; }
    public Task SetIdAsync(int id, string key = UserFieldNames.Id,
        CancellationToken cancellationToken = default);
    public Task SetEmailAsync(string email, string key = UserFieldNames.Email,
        CancellationToken cancellationToken = default);
    public Task SetRolesAsync(IEnumerable<Role> roles, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default);
    public Task SetPermissionAsync(IEnumerable<Permission> permissions, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default);
    public Task SetEmailVerifiedAsync(bool emailVerified, string key = UserFieldNames.EmailVerified,
        CancellationToken cancellationToken = default);
}