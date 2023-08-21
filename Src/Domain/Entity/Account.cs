using Domain.Common;

namespace Domain.Entity;

public class Account : BaseEntity
{
    public string Login { get; set; } = string.Empty;
    public DateTime CreateDt { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public User Owner { get; set; } = null!;
}