using Application.Common.Constants;
using Domain.Constants;
using Domain.Entities;

namespace Application.Toasts.Queries.Models;

public class ToastSelectModel
{
    public ToastSelectModel(Toast toast)
    {
        Id = toast.Id;
        Author = toast.Author;
        Type = toast.Type;
        Context = toast.Context;
        LastModified = toast.LastModified;
        Created = toast.Created;
        if (toast.Reply != null) Reply = new ToastSelectModel(toast.Reply);
        if (toast.Quote != null) Quote = new ToastSelectModel(toast.Quote);
        if (toast.ReToast != null) ReToast = new ToastSelectModel(toast.ReToast);
        MediaItems = toast.MediaItems;
    }
    
    public readonly int Id;
    public readonly Account? Author;
    public readonly string Type;
    public readonly string Context;
    public readonly DateTime LastModified;
    public readonly DateTime Created;
    public readonly ToastSelectModel? Reply;
    public readonly ToastSelectModel? Quote;
    public readonly ToastSelectModel? ReToast;
    public readonly IEnumerable<MediaItem> MediaItems;
    public bool ImReacted;
    public int ReactionsCount;
    public int RepliesCount;
    public int ReToastsCount;
    public int QuotesCount;
    public IEnumerable<ToastSelectModel> Thread = null!;
    
    public static IEnumerable<ToastSelectModel> SelectToasts(IEnumerable<Toast> toasts, int myAccountId)
    {
        return toasts.Select(toast =>
        {
            var tsm = new ToastSelectModel(toast);
            var accountId = toast.AuthorId;
            if (toast.Type != ToastType.ReToast)
            {
                tsm.Thread = SelectToasts(toast.Replies.Where(reply => reply.AuthorId == accountId), myAccountId);
                tsm.ReactionsCount = toast.Reactions.Count;
                tsm.RepliesCount = toast.Replies.Count;
                tsm.ReToastsCount = toast.ReToasts.Count;
                tsm.QuotesCount = toast.Quotes.Count;
                
                if(myAccountId != UserDefaultValues.Id)
                {
                    tsm.ImReacted = toast.Reactions.Any(r => r.AuthorId == myAccountId);
                }
            }

            return tsm;
        });
    }
    
    public static IEnumerable<ToastSelectModel> SelectReplies(IEnumerable<Toast> toasts, int myAccountId)
    {
        return toasts.Select(toast =>
        {
            var tsm = new ToastSelectModel(toast);

            if (toast.Type != ToastType.ReToast)
            {
                tsm.ReactionsCount = toast.Reactions.Count;
                tsm.RepliesCount = toast.Replies.Count;
                tsm.ReToastsCount = toast.ReToasts.Count;
                tsm.QuotesCount = toast.Quotes.Count;
                
                if(myAccountId != UserDefaultValues.Id)
                {
                    tsm.ImReacted = toast.Reactions.Any(r => r.AuthorId == myAccountId);
                }
            }

            return tsm;
        });
    }
    public static IEnumerable<ToastSelectModel> SelectQuotes(IEnumerable<Toast> toasts, int myAccountId)
    {
        return toasts.Select(toast =>
        {
            var tsm = new ToastSelectModel(toast);

            if (toast.Type != ToastType.ReToast)
            {
                tsm.ReactionsCount = toast.Reactions.Count;
                tsm.RepliesCount = toast.Replies.Count;
                tsm.ReToastsCount = toast.ReToasts.Count;
                tsm.QuotesCount = toast.Quotes.Count;
                
                if(myAccountId != UserDefaultValues.Id)
                {
                    tsm.ImReacted = toast.Reactions.Any(r => r.AuthorId == myAccountId);
                }
            }

            return tsm;
        });
    }
}