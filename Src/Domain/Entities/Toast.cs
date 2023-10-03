namespace Domain.Entities;

public class Toast : BaseAuditableEntity
{
    public int AuthorId { get; set; }
    public Account Author { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    
    public int? QuoteId { get; set; }
    public Toast? Quote { get; set; }
    public ICollection<MediaItem>? MediaItems { get; set; }
    public ICollection<Reaction>? Reactions { get; set; }
    public ICollection<Account>? ReToasters { get; set; }
}