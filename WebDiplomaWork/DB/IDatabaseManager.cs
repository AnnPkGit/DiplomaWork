using CommonLibrary;
using WebDiplomaWork.DB.DTOs;

namespace WebDiplomaWork.DB
{
    public interface IDatabaseManager
    {
        Result AddUser(UserDto userDto);
    }
}
