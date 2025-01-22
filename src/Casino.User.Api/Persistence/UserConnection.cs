using Dapper;
using System.Data;

namespace Casino.User.Api.Persistence
{
  public class UserConnection : IUserConnection
  {
    private readonly IDbConnection _connection;

    public UserConnection(IDbConnection connection)
    {
      _connection = connection;
    }

    public Task<int> ExecuteAsync(string sql, object? param = null)
    {
      return _connection.ExecuteAsync(sql, param);
    }

    public Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null)
    {
      return _connection.ExecuteScalarAsync<T>(sql, param);
    }

    public Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null)
    {
      return _connection.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    public IUserTransaction BeginTransaction()
    {
      var transaction = _connection.BeginTransaction();
      return new UserTransaction(transaction);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      _connection.Dispose();
    }
  }
}
