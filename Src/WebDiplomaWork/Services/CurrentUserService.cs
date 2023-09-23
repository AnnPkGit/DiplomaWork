using Application.Common.Constants;
using Application.Common.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;

namespace WebDiplomaWork.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly ISession _session;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _session = httpContextAccessor.HttpContext?.Session!;
    }
    public int Id => _session.GetInt32(UserFieldNames.Id) ?? UserDefaultValues.Id;
    public string Email => _session.GetString(UserFieldNames.Email) ?? UserDefaultValues.Email;

    public bool? EmailVerified
    {
        get
        {
            var res = _session.GetString(UserFieldNames.EmailVerified) ?? null;
            return res != null ? bool.Parse(res) : null;
        }
    }

    public HashSet<string> Roles
    {
        get
        {
            var res = _session.GetObjectFromJson<HashSet<string>>(UserFieldNames.Roles);
            return res ?? new HashSet<string>();
        }
    }

    public HashSet<string> Permission
    {
        get
        {
            var res = _session.GetObjectFromJson<HashSet<string>>(UserFieldNames.Permissions);
            return res ?? new HashSet<string>();
        }
    }

    public async Task SetIdAsync(int id, string key = UserFieldNames.Id,
        CancellationToken cancellationToken = default)
    {
        await _session.LoadAsync(cancellationToken);
        _session.SetInt32(key, id);
    }

    public async Task SetEmailAsync(string email, string key = UserFieldNames.Email,
        CancellationToken cancellationToken = default)
    {
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, email);
    }

    public async Task SetRolesAsync(IEnumerable<Role> roles, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default)
    {
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, JsonConvert.SerializeObject(roles.Select(role => role.ToString())));
    }

    public async Task SetPermissionAsync(IEnumerable<Permission> permissions, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default)
    {
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, JsonConvert.SerializeObject(permissions.Select(permission => permission.ToString())));
    }

    public async Task SetEmailVerifiedAsync(bool emailVerified, string key = UserFieldNames.EmailVerified,
        CancellationToken cancellationToken = default)
    {
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, emailVerified.ToString());
    }
}