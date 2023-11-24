namespace Domain.Entities;

public class Follow
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    public int ToAccountId { get; set; }
    public DateTime DateOfFollow { get; set; }
    public Account ToAccount { get; set; } = null!;
    
}
