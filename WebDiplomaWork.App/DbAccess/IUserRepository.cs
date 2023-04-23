using WebDiplomaWork.Domain.Entities;

namespace WebDiplomaWork.App.DbAccess
{
    public interface IUsersRepository
    {
        UserEntity GetById(String id);
    }
}
