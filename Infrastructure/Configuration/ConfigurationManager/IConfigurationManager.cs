namespace Infrastructure.Configuration.ConfigurationManager;

public interface IConfigurationManager
{
    DbConfiguration DbConfiguration { get; }
}