using Infrastructure.Configuration.ConfigurationManager;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;

namespace Infrastructure.DbAccess.SshAccess
{
    public class SshConnectionProvider : ISshConnectionProvider
    {
        private readonly IConfigurationManager _configurationManager;

        public SshConnectionProvider(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public MySqlConnection CreateConnectionToDb(uint localPort)
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
            {
                Server = _configurationManager.DbConfiguration.Server,
                Port = localPort,
                UserID = _configurationManager.DbConfiguration.DatabaseUserName,
                Password = _configurationManager.DbConfiguration.DatabasePassword,
                Database = _configurationManager.DbConfiguration.DatabaseName
            };

            return new MySqlConnection(csb.ConnectionString);
        }

        public (SshClient SshClient, uint Port) ConnectSsh()
        {
            var authenticationMethods = new List<AuthenticationMethod>();

            if (!string.IsNullOrEmpty(_configurationManager.SshConfiguration.SshPassword))
            {
                authenticationMethods.Add(new PasswordAuthenticationMethod(
                    _configurationManager.SshConfiguration.SshUserName,
                    _configurationManager.SshConfiguration.SshPassword));
            }

            var sshClient = new SshClient(new ConnectionInfo(
                _configurationManager.SshConfiguration.Server,
                _configurationManager.SshConfiguration.Port,
                _configurationManager.SshConfiguration.SshUserName,
                authenticationMethods.ToArray()));

            sshClient.Connect();

            var forwardedPort = new ForwardedPortLocal(
                _configurationManager.DbConfiguration.Server,
                _configurationManager.DbConfiguration.Host,
                (uint)_configurationManager.DbConfiguration.Port);

            sshClient.AddForwardedPort(forwardedPort);
            forwardedPort.Start();
            return (sshClient, forwardedPort.BoundPort);
        }
    }
}
