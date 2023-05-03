using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;

namespace WebDiplomaWork.Infrastructure.DbAccess;


public class GenericDataContext<T, K> :
    DbContext,
    IRepository<T, K>
    where T : class
    where K : class
{
    private readonly ISshConnectionProvider _shhConnectionProvider;
    private SshClient _sshClient;
    
    private DbSet<T> _table { get; set; }

    public GenericDataContext(ISshConnectionProvider shhConnectionProvider)
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

    public IQueryable<T> GetAll()
    {
        return _table.AsQueryable();
    }

    public T GetById(K id)
    {
        return _table.Find(id);
    }

    public T Create(T entity)
    {
        return _table.Add(entity).Entity;
    }

    public T Update(T entity)
    {
        return _table.Update(entity).Entity;
    }

    public T Delete(T entity)
    {
        return _table.Remove(entity).Entity;
    }

    public void Save()
    {
        SaveChanges();
    }
}