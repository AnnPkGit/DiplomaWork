namespace Domain.Entities;

public class Account : BaseAuditableEntity
{
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public int? AvatarId { get; set; }
    public AvatarMediaItem? Avatar { get; set; }
    public string? Bio { get; set; }
    public User Owner { get; set; } = null!;
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> Follows { get; set; } = new List<Follow>();
}