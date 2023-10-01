namespace Domain.Entities;

public class Account : BasicLegalEntity
{
    public string Login { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public User Owner { get; set; } = null!;
}