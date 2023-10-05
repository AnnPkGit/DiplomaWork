namespace Infrastructure.Configurations;

public class AzureStorageConfig
{
    public string AccountName { get; init; } = string.Empty;
    public string ImageContainer { get; init; } = string.Empty;
    public string AccountKey { get; init; } = string.Empty;
}