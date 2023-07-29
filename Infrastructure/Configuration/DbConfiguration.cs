namespace Infrastructure.Configuration;

public class DbConfiguration
{
    public int Port { get; set; }
    public string? Host { get; set; }
    public string? DatabaseUserName { get; set; }
    public string? DatabaseName { get; set; }
    public string? DatabasePassword { get; set; }
}