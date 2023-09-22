namespace Domain.Entities;

public class Account : BaseAuditableEntity
{
    public string Login { get; set; } = string.Empty;
    
    public new DateTime Created { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}