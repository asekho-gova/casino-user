namespace Casino.User.Api.Persistence
{
  public interface IUserConnection : IDisposable
  {
    Task<int> ExecuteAsync(string sql, object? param = null);

    Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null);

    Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null);

    IUserTransaction BeginTransaction();
  }
}
