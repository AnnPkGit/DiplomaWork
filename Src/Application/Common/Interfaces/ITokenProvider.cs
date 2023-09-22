using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ITokenProvider
{
    string GenAccessToken(User user);
    string GenRefreshToken();
    string GetEmailVerifyToken(int userId, string email);
}