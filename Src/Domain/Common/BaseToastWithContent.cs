using System.Reflection;

namespace Domain.Common;

public class BaseToastWithContent : BaseToast
{
    public BaseToastWithContent()
    {
    }

    protected BaseToastWithContent(int authorId, string content, MemberInfo childType) : base(authorId, childType)
    {
        Content = content;
    }
    
    protected BaseToastWithContent(int authorId, string content, MemberInfo childType, params ToastMediaItem[] mediaItems) : base(authorId, childType)
    {
        Content = content;
        MediaItems = mediaItems;
    }

    public string Content { get; set; } = string.Empty;
    public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public ICollection<ReToast> ReToasts { get; set; } = new List<ReToast>();
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<ToastMediaItem> MediaItems { get; set; } = new List<ToastMediaItem>();
}