namespace App.Repository;

public interface IRepository<T, K>
    where T : class
{
    IQueryable<T> GetAll();
    T GetById(K id);
    Task CreateAsync(T entity);
    T Update(T entity);
    T Delete(T entity);
    Task SaveAsync();
}