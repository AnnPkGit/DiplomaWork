using Microsoft.Extensions.Options;

namespace Infrastructure.Configuration.ConfigurationManager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public SshConfiguration? SshConfiguration { get; }

        public DbConfiguration? DbConfiguration { get; }

        public ConfigurationManager(IOptionsSnapshot<GeneralConfiguration> generalConfiguration)
        {
            SshConfiguration = generalConfiguration.Value.SshConfiguration;
            DbConfiguration = generalConfiguration.Value.DbConfiguration;
        }
    }
}
