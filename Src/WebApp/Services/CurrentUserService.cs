using Application.Common.Constants;
using Application.Common.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;

namespace WebApp.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IServiceProvider _serviceProvider;
    private ISession _session;

    public CurrentUserService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _session = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.Session!;
    }
    public int Id
    {
        get
        {
            Reset();
            return _session.GetInt32(UserFieldNames.Id) ?? UserDefaultValues.Id;
        }
    }

    public string Email
    {
        get
        {
            Reset();
            return _session.GetString(UserFieldNames.Email) ?? UserDefaultValues.Email;
        }
    }

    public bool? EmailVerified
    {
        get
        {
            Reset();
            var res = _session.GetString(UserFieldNames.EmailVerified) ?? null;
            return res != null ? bool.Parse(res) : null;
        }
    }

    public HashSet<string> Roles
    {
        get
        {
            Reset();
            var res = _session.GetObjectFromJson<HashSet<string>>(UserFieldNames.Roles);
            return res ?? new HashSet<string>();
        }
    }

    public HashSet<string> Permission
    {
        get
        {
            Reset();
            var res = _session.GetObjectFromJson<HashSet<string>>(UserFieldNames.Permissions);
            return res ?? new HashSet<string>();
        }
    }

    public async Task SetIdAsync(int id, string key = UserFieldNames.Id,
        CancellationToken cancellationToken = default)
    {
        Reset();
        await _session.LoadAsync(cancellationToken);
        _session.SetInt32(key, id);
    }

    public async Task SetEmailAsync(string email, string key = UserFieldNames.Email,
        CancellationToken cancellationToken = default)
    {
        Reset();
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, email);
    }

    public async Task SetRolesAsync(IEnumerable<Role> roles, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default)
    {
        Reset();
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, JsonConvert.SerializeObject(roles.Select(role => role.ToString())));
    }

    public async Task SetPermissionsAsync(IEnumerable<Permission> permissions, string key = UserFieldNames.Roles,
        CancellationToken cancellationToken = default)
    {
        Reset();
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, JsonConvert.SerializeObject(permissions.Select(permission => permission.ToString())));
    }

    public async Task SetEmailVerifiedAsync(bool emailVerified, string key = UserFieldNames.EmailVerified,
        CancellationToken cancellationToken = default)
    {
        Reset();
        await _session.LoadAsync(cancellationToken);
        _session.SetString(key, emailVerified.ToString());
    }

    public Task Clear(CancellationToken cancellationToken = default)
    {
        Reset();
        _session.Clear();
        return Task.CompletedTask;
    }

    private void Reset()
    {
        _session = _serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.Session!;
    }

    public async Task SetAllPropertiesAsync(
        int id,
        string email,
        bool emailVerified,
        IEnumerable<Role> roles,
        IEnumerable<Permission> permissions,
        CancellationToken cancellationToken = default)
    {
        const string idKey = UserFieldNames.Id;
        const string emailKey = UserFieldNames.Email;
        const string emailVerifiedKey = UserFieldNames.EmailVerified;
        const string rolesKey = UserFieldNames.Roles;
        const string permissionsKey = UserFieldNames.Permissions;
        
        Reset();
        await _session.LoadAsync(cancellationToken);
        
        _session.SetInt32(idKey, id);
        _session.SetString(emailKey, email);
        _session.SetString(emailVerifiedKey, emailVerified.ToString());
        _session.SetString(rolesKey, JsonConvert.SerializeObject(roles.Select(role => role.ToString())));
        _session.SetString(permissionsKey, JsonConvert.SerializeObject(permissions.Select(permission => permission.ToString())));
    }
}