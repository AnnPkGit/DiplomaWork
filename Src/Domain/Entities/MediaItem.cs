namespace Domain.Entities;

public class MediaItem
{
    public MediaItem(){}

    public MediaItem(string url, string name, string type, int authorId)
    {
        Url = url;
        Name = name;
        Type = type;
        AuthorId = authorId;
    }

    public int Id { get; set; }
    public int? AuthorId { get; set; }
    public Account? Author { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    public ICollection<Toast> Toasts { get; set; } = null!;
}