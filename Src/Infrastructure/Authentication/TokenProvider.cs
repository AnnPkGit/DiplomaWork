using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    private readonly AccessTokenOptions _accessOptions;
    private readonly EmailVerifyTokenOptions _emailVerifyOptions;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        var jwtOptions = options.Value;
        _accessOptions = jwtOptions.AccessToken;
        _emailVerifyOptions = jwtOptions.EmailVerifyToken;
    }

    public string GenAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_accessOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.UtcNow.Add(TimeSpan.Parse(_accessOptions.Lifetime));
        
        var token = new JwtSecurityToken(
            _accessOptions.Issuer,
            _accessOptions.Audience,
            claims,
            null,
            expires,
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);
        return tokenValue;
    }

    public string GenRefreshToken()
    {
        return "Not implemented";
    }
    
    public string GetEmailVerifyToken(int userId, string email)
    {
        var claims = new Claim[]
        {
            new (JwtRegisteredClaimNames.NameId, userId.ToString()),
            new (JwtRegisteredClaimNames.Email, email)
        };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_emailVerifyOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.UtcNow.Add(TimeSpan.Parse(_emailVerifyOptions.Lifetime));
        
        var token = new JwtSecurityToken(
            _emailVerifyOptions.Issuer,
            _emailVerifyOptions.Audience,
            claims,
            null,
            expires,
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);
        return tokenValue;
    }
}