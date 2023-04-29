using MySql.Data.MySqlClient;
using Renci.SshNet;

namespace WebDiplomaWork.Infrastructure.DbAccess.SshAccess
{
    public interface ISshConnectionProvider
    {
        (SshClient SshClient, uint Port) ConnectSsh();

        MySqlConnection CreateConnectionToDb(uint localPort);
    }
}
