namespace Domain.Entities;

public class Reaction
{
    public Reaction()
    {
    }

    public Reaction(int toastWithContentId, int authorId, DateTime reacted)
    {
        ToastWithContentId = toastWithContentId;
        AuthorId = authorId;
        Reacted = reacted;
    }

    public int Id { get; init; }
    
    public int ToastWithContentId { get; set; }
    public BaseToastWithContent ToastWithContent { get; set; } = null!;
    
    public int AuthorId { get; set; }
    public Account Author { get; set; } = null!;
    
    public string Code { get; set; } = string.Empty;
    public DateTime Reacted { get; set; }
}