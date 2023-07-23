using Infrastructure.DbAccess.Entity;
using Infrastructure.DbAccess.SshAccess;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;

namespace Infrastructure.DbAccess.EfDbContext;

public class DataContext : DbContext
{
    private readonly ISshConnectionProvider _shhConnectionProvider;
    private SshClient _sshClient;
    public DbSet<UserEntity> Users { get; set; }

    public DataContext(ISshConnectionProvider shhConnectionProvider)
    {
        _shhConnectionProvider = shhConnectionProvider;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var (sshClient, localPort) = _shhConnectionProvider.ConnectSsh();
        _sshClient = sshClient;
        var connection = _shhConnectionProvider.CreateConnectionToDb(localPort);
        optionsBuilder.UseMySQL( connection );
    }

    public override void Dispose()
    {
        base.Dispose();
        _sshClient.Dispose();
    }
}