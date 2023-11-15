namespace Domain.Entities;

public class ToastMediaItem : BaseMediaItem
{
    public ToastMediaItem() { }

    public ToastMediaItem(string url, string name, string type, int authorId) : base(url, name, type, authorId)
    {
    }
}