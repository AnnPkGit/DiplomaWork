using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;

namespace WebDiplomaWork.Infrastructure.DbAccess;


public class GenericRepository<T, K> :
    IRepository<T, K>
    where T : class
{
    private readonly DataContext _dataContext;
    private readonly DbSet<T> _table;

    public GenericRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _table = _dataContext.Set<T>();
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
        _dataContext.SaveChanges();
    }
}