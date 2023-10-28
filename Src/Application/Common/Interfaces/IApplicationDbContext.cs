using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    
    DbSet<Account> Accounts { get; }
    
    DbSet<MediaItem> MediaItems { get; }
    
    DbSet<Reaction> Reactions { get; }
    
    DbSet<BaseToast> BaseToasts { get; }
    
    DbSet<BaseToastWithContent> BaseToastsWithContent { get; }
    
    DbSet<Toast> Toasts { get; }
    
    DbSet<ReToast> ReToasts { get; }
    
    DbSet<Reply> Replies { get; }
    
    DbSet<Quote> Quotes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}