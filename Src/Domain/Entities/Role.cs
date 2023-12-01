namespace Domain.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role SuperAdministrator = new(1, nameof(SuperAdministrator));
    public static readonly Role Administrator = new(2, nameof(Administrator));
    public static readonly Role Registered = new(3, nameof(Registered));
    public static readonly Role Verified = new(4, nameof(Verified));
    private Role(int id, string name) : base(id, name)
    {
    }
    public ICollection<Permission> Permissions { get; set; } = null!;
}