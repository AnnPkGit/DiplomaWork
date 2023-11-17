namespace Domain.Entities;

public class Reply : BaseToastWithContent
{
    public Reply(){}

    public Reply(int authorId, string content, int replyToToastId)
        : base(authorId, content, typeof(Reply))
    {
        ReplyToToastId = replyToToastId;
    }

    public Reply(int authorId, string content, int replyToToastId, params ToastMediaItem[] mediaItems)
        : base(authorId, content, typeof(Reply), mediaItems)
    {
        ReplyToToastId = replyToToastId;
    }

    public int? ReplyToToastId;
    public BaseToastWithContent? ReplyToToast { get; set; } = null!;
}