namespace Infrastructure.Authentication;

public sealed class JwtOptions
{
    public AccessTokenOptions AccessToken { get; init; } = null!;
    public EmailVerifyTokenOptions EmailVerifyToken { get; init; } = null!;
}