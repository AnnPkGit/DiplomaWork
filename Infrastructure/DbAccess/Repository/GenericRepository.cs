using App.Repository;
using Infrastructure.DbAccess.EfDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbAccess.Repository;

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

    public async Task CreateAsync(T entity)
    {
        await _table.AddAsync(entity);
    }

    public T Update(T entity)
    {
        return _table.Update(entity).Entity;
    }

    public T Delete(T entity)
    {
        return _table.Remove(entity).Entity;
    }

    public async Task SaveAsync()
    {
        await _dataContext.SaveChangesAsync();
    }
}