using CommonLibrary;
using WebDiplomaWork.Domain.Entities;

namespace WebDiplomaWork.App
{
    public interface IUserService
    {
        Task<Result> AddUserAsync(UserEntity user);
    }
}
