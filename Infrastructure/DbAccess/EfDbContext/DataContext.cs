using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess.EfDbContext;

public class DataContext : DbContext
{
    private readonly IDbAccessProvider _dbAccessProvider;
    public DbSet<UserEntity> Users { get; set; }

    public DataContext(IDbAccessProvider dbAccessProvider)
    {
        _dbAccessProvider = dbAccessProvider;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _dbAccessProvider.GetConnectionString();
        optionsBuilder.UseMySQL(connectionString);
    }
}