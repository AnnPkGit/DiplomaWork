namespace Domain.Entities;

public class Reaction
{
    public int Id { get; init; }
    public int MediaItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public MediaItem MediaItem { get; set; } = null!;
}