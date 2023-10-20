using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    
    DbSet<Account> Accounts { get; }
    
    DbSet<MediaItem> MediaItems { get; }
    
    DbSet<Toast> Toasts { get; }
    
    DbSet<Reaction> Reactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}