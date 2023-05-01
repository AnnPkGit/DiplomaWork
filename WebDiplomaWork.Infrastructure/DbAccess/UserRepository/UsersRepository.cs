using MySql.Data.MySqlClient;
using WebDiplomaWork.App.DbAccess;
using WebDiplomaWork.Domain.Entities;
using WebDiplomaWork.Infrastructure.DbAccess.SshAccess;

namespace WebDiplomaWork.Infrastructure.DbAccess.UserRepository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ISshConnectionProvider _shhConnectionProvider;

        private delegate object SqlResponseReader(MySqlDataReader reader);

        private readonly SqlResponseReader _readUserAction;

        public UsersRepository(ISshConnectionProvider shhConnectionProvider)
        {
            _shhConnectionProvider = shhConnectionProvider;
            _readUserAction = ReadUser;
        }

        public UserEntity GetById(string id)
        {
            return (UserEntity)CreateDbConditionsAndExecuteSql($"SELECT * FROM Users WHERE Id = '{id}'", _readUserAction);
        }

        private UserEntity ReadUser(MySqlDataReader reader)
        {
            UserEntity user = null;
            while (reader.Read())
            {
                user = CreateUSerFromReader(reader);
            }
            return user;
        }

        private object CreateDbConditionsAndExecuteSql(string sqlCommand, SqlResponseReader action)
        {
            object result;
            var (sshClient, localPort) = _shhConnectionProvider.ConnectSsh();
            using (sshClient)
            {
                using (var connection = _shhConnectionProvider.CreateConnectionToDb(localPort))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sqlCommand, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            result = action(reader);
                        }
                    }
                    connection.Close();
                    return result;
                }
            }
        }

        private UserEntity CreateUSerFromReader(MySqlDataReader reader)
        {
            try
            {
                var user = new UserEntity()
                {
                    Id = reader.GetValue(0).ToString(),
                    Login = reader.GetValue(1).ToString(),
                    RegistrationDt = DateTime.Parse(reader.GetValue(2).ToString()),
                    BirthDate = DateTime.Parse(reader.GetValue(3).ToString()),
                    Name = reader.GetValue(4).ToString(),
                    Email = reader.GetValue(5).ToString(),
                    EmailVerified = reader.GetInt32(6),
                    Phone = reader.GetValue(7).ToString(),
                    PhoneVerified = reader.GetInt32(8),
                    Avatar = reader.GetValue(9).ToString(),
                    Bio = reader.GetValue(10).ToString(),
                };
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
