using WebDiplomaWork.App.DbAccess;

namespace WebDiplomaWork.Infrastructure.DbAccess
{
    internal class Repository<T> : IRepository<T>
    {
        public void Delete(Guid iD)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
