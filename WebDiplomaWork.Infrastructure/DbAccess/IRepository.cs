namespace WebDiplomaWork.Infrastructure.DbAccess;

public interface IRepository<T, K>
: IDisposable
    where T : class
    where K : class
{
    public IQueryable<T> GetAll();

    public T GetById(K id);
    public T Create(T entity);
    public T Update(T entity);
    public T Delete(T entity);
    public void Save();
    
    void IDisposable.Dispose() {}
}