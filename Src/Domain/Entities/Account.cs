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
    public ICollection<BaseToast> AllToasts { get; set; } = new List<BaseToast>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<BaseMediaItem> MediaItems { get; set; } = new List<BaseMediaItem>();
    
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> MyFollows { get; set; } = new List<Follow>();
}