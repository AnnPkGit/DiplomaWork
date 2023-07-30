namespace Infrastructure.Configuration;

public class DbConfiguration
{
    public int Port { get; set; }
    public string? Server { get; set; }
    public string? UserId { get; set; }
    public string? Database { get; set; }
    public string? Password { get; set; }
    public string? ServerVersion { get; set; }
}