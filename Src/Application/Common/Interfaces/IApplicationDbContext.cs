using Domain.Common;
using Domain.Entities;
using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    
    DbSet<Account> Accounts { get; }
    
    DbSet<Reaction> Reactions { get; }
    
    DbSet<BaseToast> BaseToasts { get; }
    
    DbSet<BaseToastWithContent> BaseToastsWithContent { get; }
    
    DbSet<Toast> Toasts { get; }
    
    DbSet<ReToast> ReToasts { get; }
    
    DbSet<Reply> Replies { get; }
    
    DbSet<Quote> Quotes { get; }
    
    DbSet<BaseNotification> BaseNotifications { get; }
    DbSet<ReactionNotification> ReactionNotifications { get; }
    DbSet<ReToastNotification> ReToastNotifications { get; }
    DbSet<QuoteNotification> QuoteNotifications { get; }
    DbSet<ReplyNotification> ReplyNotifications { get; }
    DbSet<ToastMediaItem> ToastMediaItems { get; }
    DbSet<AvatarMediaItem> AvatarMediaItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}