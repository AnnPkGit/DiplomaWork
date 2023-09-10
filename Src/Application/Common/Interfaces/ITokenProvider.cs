using Domain.Entity;

namespace Application.Common.Interfaces;

public interface ITokenProvider
{
    string GenAccessToken(User user);
    string GenRefreshToken();
}