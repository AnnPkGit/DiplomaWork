using Domain.Entity;

namespace App.Common;

public interface IJwtProvider
{
    string Generate(User user);
}