namespace Domain.Entities;

public class Toast : BaseToastWithContent
{
    public Toast()
    {
    }

    public Toast(int authorId, string content)
        : base(authorId, content, typeof(Toast))
    {
    }

    public Toast(int authorId, string content, params ToastMediaItem[] mediaItems)
        : base(authorId, content, typeof(Toast), mediaItems)
    {
    }
}