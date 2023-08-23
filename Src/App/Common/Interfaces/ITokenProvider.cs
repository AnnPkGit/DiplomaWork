using Domain.Entity;

namespace App.Common.Interfaces;

public interface ITokenProvider
{
    string GenAccessToken(User user);
    string GenRefreshToken();
}