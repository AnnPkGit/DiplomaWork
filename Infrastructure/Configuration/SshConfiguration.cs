namespace Infrastructure.Configuration;

public class SshConfiguration
{
    public String? Server { get; set; }

    public int Port { get; set; }

    public String? SshUserName { get; set; }

    public String? SshPassword { get; set; }
}