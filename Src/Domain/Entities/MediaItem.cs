namespace Domain.Entities;

public class MediaItem
{
    public MediaItem(){}

    public MediaItem(string url, string name, string type)
    {
        Url = url;
        Name = name;
        Type = type;
    }

    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}