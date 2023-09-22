namespace Application.Common.Interfaces;

public interface ITokenValidator
{
    bool ValidateEmailVerifyToken(string token, out (int userId, string email) claims );
}