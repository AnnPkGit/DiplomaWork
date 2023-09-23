namespace Infrastructure.Configuration;

public class GeneralConfiguration
{
    public DbConfiguration DbConfiguration { get; set; } = null!;
    public EmailSettings EmailSettings { get; set; } = null!;
}