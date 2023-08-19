using Domain.Entity;

namespace App.Common;

public interface ITokenProvider
{
    string GenAccessToken(User user);
    string GenRefreshToken();
}