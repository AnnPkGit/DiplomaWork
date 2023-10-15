namespace Domain.Entities;

public class Toast : BaseAuditableEntity
{
    public Toast() {}
    
    private Toast(int authorId, string context, string type)
    {
        AuthorId = authorId;
        Context = context;
        Type = type;
    }
    
    private Toast(int authorId, string context, string type, params MediaItem[] mediaItems)
    {
        AuthorId = authorId;
        Context = context;
        Type = type;
        MediaItems = mediaItems;
    }

    public int? AuthorId { get; set; }
    public Account? Author { get; set; }
    public string Context { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    
    public int? ReplyId { get; set; }
    public Toast? Reply { get; set; }
    
    public int? QuoteId { get; set; }
    public Toast? Quote { get; set; }
    public ICollection<MediaItem>? MediaItems { get; set; }
    public ICollection<Reaction>? Reactions { get; set; }
    public ICollection<Account>? ReToasters { get; set; }
    public ICollection<Toast>? Quotes { get; set; }
    public ICollection<Toast>? Replies { get; set; }

    public static Toast CreateToast(int authorId, string context, params MediaItem[] mediaItems)
    {
        const string type = "toast";
        
        var res = mediaItems.Length == 0
            ? new Toast(authorId, context, type)
            : new Toast(authorId, context, type, mediaItems);

        return res;
    }
    
    public static Toast CreateReply(int authorId, string context, int toastId, params MediaItem[] mediaItems)
    {
        const string type = "reply";
        Toast res;
        
        if (mediaItems.Length == 0)
        {
            res = new Toast(authorId, context, type)
            {
                ReplyId = toastId
            };
        }
        else
        {
            res = new Toast(authorId, context, type, mediaItems)
            {
                ReplyId = toastId
            };
        }

        return res;
    }
    
    public static Toast CreateQuote(int authorId, string context, int toastId, params MediaItem[] mediaItems)
    {
        const string type = "quote";
        Toast res;
        
        if (mediaItems.Length == 0)
        {
            res = new Toast(authorId, context, type)
            {
                QuoteId = toastId
            };
        }
        else
        {
            res = new Toast(authorId, context, type, mediaItems)
            {
                QuoteId = toastId
            };
        }

        return res;
    }
}