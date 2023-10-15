namespace Domain.Entities;

public class Account : BaseAuditableEntity
{
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public User Owner { get; set; } = null!;
    public ICollection<Toast>? Toasts { get; set; }
    public ICollection<Toast>? ReToasts { get; set; }
    public ICollection<Reaction>? Reactions { get; set; }
    public ICollection<MediaItem>? MediaItems { get; set; }
}