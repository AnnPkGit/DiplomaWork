using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess.EfDbContext;

public class ExampleContext : DbContext
{
    private readonly IDbAccessProvider _dbAccessProvider;
    public DbSet<ExampleEntity> Example { get; private set; }
    
    public ExampleContext(
        IDbAccessProvider dbAccessProvider,
        DbContextOptions options) : base(options)
    {
        _dbAccessProvider = dbAccessProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _dbAccessProvider.GetConnectionString();
        var serverVersion = _dbAccessProvider.GetServerVersion();
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}