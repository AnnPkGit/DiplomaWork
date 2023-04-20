namespace WebDiplomaWork.App.DbAccess
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int entityId);
        void Insert(T entity);
        void Delete(Guid iD);
        void Update(T entity);
        void Save();
    }
}
