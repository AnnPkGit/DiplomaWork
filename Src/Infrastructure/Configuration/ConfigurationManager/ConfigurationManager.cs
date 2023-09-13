using Microsoft.Extensions.Options;

namespace Infrastructure.Configuration.ConfigurationManager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public DbConfiguration DbConfiguration { get; }
        public EmailSettings EmailSettings { get; }
        
        public ConfigurationManager(IOptionsSnapshot<GeneralConfiguration> generalConfiguration)
        {
            DbConfiguration = generalConfiguration.Value.DbConfiguration;
            EmailSettings = generalConfiguration.Value.EmailSettings;
        }
    }
}
