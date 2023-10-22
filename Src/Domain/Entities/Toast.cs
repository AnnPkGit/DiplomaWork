using Domain.Constants;

namespace Domain.Entities;

public class Toast : BaseAuditableEntity
{
    public Toast() {}

    public Toast(Toast toast)
    {
        Id = toast.Id;
        base.Deactivated = toast.Deactivated;
        DeactivatedById = toast.DeactivatedById;
        Created = toast.Created;
        LastModified = toast.LastModified;
        AuthorId = toast.AuthorId;
        Context = toast.Context;
        Type = toast.Type;
        ReplyId = toast.ReplyId;
        QuoteId = toast.QuoteId;
        Author = toast.Author;
        MediaItems = toast.MediaItems;
    }

    private Toast(int authorId, string context, string type)
    {
        AuthorId = authorId;
        Context = context;
        Type = type;
    }
    
    private Toast(int authorId, string type)
    {
        AuthorId = authorId;
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
    
    public int? ReToastId { get; set; }
    public Toast? ReToast { get; set; }
    public ICollection<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    //public ICollection<Account> ReToasters { get; set; } = new List<Account>();
    public ICollection<Toast> Quotes { get; set; } = new List<Toast>();
    public ICollection<Toast> Replies { get; set; } = new List<Toast>();
    public ICollection<Toast> ReToasts { get; set; } = new List<Toast>();
    public static Toast CreateToast(int authorId, string context, params MediaItem[] mediaItems)
    {
        var type = ToastType.Toast;
        
        var res = mediaItems.Length == 0
            ? new Toast(authorId, context, type)
            : new Toast(authorId, context, type, mediaItems);

        return res;
    }
    
    public static Toast CreateReply(int authorId, string context, int toastId, params MediaItem[] mediaItems)
    {
        var type = ToastType.Reply;
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
        var type = ToastType.Quote;
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
    
    public static Toast CreateReToast(int authorId, int toastId)
    {
        var type = ToastType.ReToast;

        var res = new Toast(authorId, type)
        {
            ReToastId = toastId
        };

        return res;
    }
}