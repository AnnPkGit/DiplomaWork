namespace Infrastructure.Configurations;

public abstract class BaseTokenOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string Lifetime { get; init; } = string.Empty;
}