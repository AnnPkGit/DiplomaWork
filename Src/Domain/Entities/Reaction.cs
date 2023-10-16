namespace Domain.Entities;

public class Reaction
{
    public int Id { get; init; }
    
    public int? ToastId { get; set; }
    public Toast? Toast { get; set; }
    
    public int? AuthorId { get; set; }
    public Account? Author { get; set; }
    
    public string Code { get; set; } = string.Empty;
    public DateTime Reacted { get; set; }
}