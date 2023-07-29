using Infrastructure.Configuration.ConfigurationManager;
using MySql.Data.MySqlClient;

namespace Infrastructure.Configuration.Provider;

public class MySqlDbAccessProvider : IDbAccessProvider
{
    private readonly IConfigurationManager _configurationManager;
    public MySqlDbAccessProvider(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public string GetConnectionString()
    {
        var csb = new MySqlConnectionStringBuilder
        {
            Server = _configurationManager.DbConfiguration.Host,
            Port = (uint)_configurationManager.DbConfiguration.Port,
            UserID = _configurationManager.DbConfiguration.DatabaseUserName,
            Password = _configurationManager.DbConfiguration.DatabasePassword,
            Database = _configurationManager.DbConfiguration.DatabaseName
        };

        return csb.ConnectionString;
    }
}