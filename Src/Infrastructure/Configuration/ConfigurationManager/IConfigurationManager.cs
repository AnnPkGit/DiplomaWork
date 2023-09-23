namespace Infrastructure.Configuration.ConfigurationManager;

public interface IConfigurationManager
{
    DbConfiguration DbConfiguration { get; }
    EmailSettings EmailSettings { get; }
}