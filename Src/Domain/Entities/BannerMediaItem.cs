namespace Domain.Entities;

public class BannerMediaItem : BaseMediaItem
{
    public BannerMediaItem()
    {
    }

    public BannerMediaItem(string url, string name, string type, int authorId) : base(url, name, type, authorId)
    {
    }
}