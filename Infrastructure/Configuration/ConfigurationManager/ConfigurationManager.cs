using Microsoft.Extensions.Options;

namespace Infrastructure.Configuration.ConfigurationManager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public DbConfiguration? DbConfiguration { get; }
        public ConfigurationManager(IOptionsSnapshot<GeneralConfiguration> generalConfiguration)
        {
            DbConfiguration = generalConfiguration.Value.DbConfiguration;
        }
    }
}
