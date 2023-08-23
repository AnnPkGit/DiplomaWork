using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace App.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ExampleItem> ExampleItems { get; }
    
    DbSet<User> Users { get; }
    
    DbSet<Account> Accounts { get;  }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}