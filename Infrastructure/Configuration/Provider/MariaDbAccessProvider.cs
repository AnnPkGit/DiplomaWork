using Infrastructure.Configuration.ConfigurationManager;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Infrastructure.Configuration.Provider;

public class MariaDbAccessProvider : IDbAccessProvider
{
    private readonly IConfigurationManager _configurationManager;
    public MariaDbAccessProvider(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public string GetConnectionString()
    {
        var csb = new MySqlConnectionStringBuilder
        {
            Server = _configurationManager.DbConfiguration.Server,
            Port = (uint)_configurationManager.DbConfiguration.Port,
            UserID = _configurationManager.DbConfiguration.UserId,
            Password = _configurationManager.DbConfiguration.Password,
            Database = _configurationManager.DbConfiguration.Database
        };

        return csb.ConnectionString;
    }

    public ServerVersion GetServerVersion()
    {
        return new MariaDbServerVersion(_configurationManager.DbConfiguration.ServerVersion);
    }
}