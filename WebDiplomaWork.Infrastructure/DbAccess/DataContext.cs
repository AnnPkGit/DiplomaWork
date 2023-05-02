using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;

namespace WebDiplomaWork.Infrastructure.DbAccess;


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