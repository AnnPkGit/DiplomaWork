using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class TokenValidator : ITokenValidator
{
    private readonly TokenValidationParameters _parameters;

    public TokenValidator(TokenValidationParameters parameters)
    {
        _parameters = parameters;
    }

    public bool ValidateEmailVerifyToken(string token, out (int userId, string email) claims)
    {
        var jwtValidator = new JwtSecurityTokenHandler();
        if(jwtValidator.CanValidateToken){}
        claims = default;
        try
        {
            var tokenClaims = jwtValidator.ValidateToken(token, _parameters, out _);
            var claimId = tokenClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            var claimEmail = tokenClaims.FindFirstValue(ClaimTypes.Email);
            
            if (claimId is null || claimEmail is null)
            {
                return false;
            }

            claims = (int.Parse(claimId), claimEmail);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}