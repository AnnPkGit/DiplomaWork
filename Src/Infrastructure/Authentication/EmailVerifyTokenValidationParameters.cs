using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class EmailVerifyTokenValidationParameters : TokenValidationParameters
{
    public EmailVerifyTokenValidationParameters(IOptions<JwtOptions> options)
    {
        var verifyOptions = options.Value.EmailVerifyToken;
        ValidateIssuer = true;
        ValidateAudience = true;
        ValidateLifetime = true;
        ValidateIssuerSigningKey = true;
        ValidIssuer = verifyOptions.Issuer;
        ValidAudience = verifyOptions.Audience;
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(verifyOptions.SecretKey));
        ClockSkew = TimeSpan.Zero;
    }
}