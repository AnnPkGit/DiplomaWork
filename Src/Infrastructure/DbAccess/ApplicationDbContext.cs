using System.Reflection;
using App.Common.Interfaces;
using Domain.Entity;
using Infrastructure.Configuration.Provider;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess;

public class ApplicationDbContext : DbContext , IApplicationDbContext
{
    private readonly IDbAccessProvider _dbAccessProvider;

    public ApplicationDbContext(
        IDbAccessProvider dbAccessProvider)
    {
        _dbAccessProvider = dbAccessProvider;
    }

    public DbSet<ExampleItem> ExampleItems => Set<ExampleItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _dbAccessProvider.GetConnectionString();
        var serverVersion = _dbAccessProvider.GetServerVersion();
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}