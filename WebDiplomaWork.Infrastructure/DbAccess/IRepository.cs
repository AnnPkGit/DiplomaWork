namespace WebDiplomaWork.Infrastructure.DbAccess;

public interface IRepository<T, K>
    where T : class
{
    public IQueryable<T> GetAll();

    public T GetById(K id);
    public T Create(T entity);
    public T Update(T entity);
    public T Delete(T entity);
    public void Save();
}