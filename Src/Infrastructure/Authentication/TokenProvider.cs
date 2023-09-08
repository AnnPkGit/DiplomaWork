using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenAccessToken(User user)
    {
        var claims = new Claim[]
        {
            new (JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };
        
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        
        var expires = DateTime.UtcNow.Add(TimeSpan.Parse(_options.Lifetime));
        
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
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
}