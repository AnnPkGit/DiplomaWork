using Domain.Common;
using Domain.Entity;

namespace App.Service;

public interface IUserService
{
    Task<Result> AddUserAsync(User user);

}