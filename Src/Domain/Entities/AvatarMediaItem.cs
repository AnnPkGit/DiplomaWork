namespace Domain.Entities;

public class AvatarMediaItem : BaseMediaItem
{
    public AvatarMediaItem() { }

    public AvatarMediaItem(string url, string name, string type, int authorId) : base(url, name, type, authorId)
    {
    }
}