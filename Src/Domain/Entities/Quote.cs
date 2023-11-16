namespace Domain.Entities;

public class Quote : BaseToastWithContent
{
    public Quote(int authorId, string content, int quotedToastId)
        : base(authorId, content, typeof(Quote))
    {
        QuotedToastId = quotedToastId;
    }
    
    public Quote(int authorId, string content, int quotedToastId, params ToastMediaItem[] mediaItems)
        : base(authorId, content, typeof(Quote), mediaItems)
    {
        QuotedToastId = quotedToastId;
    }

    public int QuotedToastId;
    public BaseToastWithContent QuotedToast { get; set; } = null!;
}