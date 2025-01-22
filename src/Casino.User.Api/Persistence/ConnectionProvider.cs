using Microsoft.Data.Sqlite;

namespace Casino.User.Api.Persistence
{
  public class ConnectionProvider : IConnectionProvider
  {
    private const string _connectionString = "Data Source=casinoUsers.db";

    public IUserConnection Open()
    {
      var sqlConnection = new SqliteConnection(_connectionString);
      sqlConnection.Open();

      return new UserConnection(sqlConnection);
    }
  }
}
