using Casino.User.Api.Models;
using System.Net;

namespace Casino.User.Api.Persistence;

public class DatabaseService : IDatabaseService
{
  private readonly IConnectionProvider _commandProvider;

  public DatabaseService(IConnectionProvider commandProvider)
  {
    _commandProvider = commandProvider;
  }

  public async Task<IUserTransaction> BeginTransactionAsync()
  {
    var connection = _commandProvider.Open();
    var transaction = connection.BeginTransaction();
    return new UserTransaction(transaction);
  }

  public async Task<int> CreateUserAsync(CreateUserRequest request)
  {
    using var connection = _commandProvider.Open();
    const string sql = @"
            INSERT INTO tb_Users (
                Username, 
                Password,
                Salt,
                Email, 
                HomePhoneNumber, 
                WorkPhoneNumber, 
                MobilePhoneNumber
            ) VALUES (
                @Username, 
                @HashedPassword,
                @Salt,
                @Email, 
                @HomePhoneNumber, 
                @WorkPhoneNumber, 
                @MobilePhoneNumber
            ); 
            SELECT last_insert_rowid();";
    var parameters = new
    {
      request.Username,
      request.HashedPassword,
      request.Email,
      request.HomePhoneNumber,
      request.WorkPhoneNumber,
      request.MobilePhoneNumber
    };

    var id = await connection.ExecuteScalarAsync<int>(sql, parameters);
    return id;
  }

  public async Task<CasinoUser?> GetUserAsync(int userId)
  {
    using var connection = _commandProvider.Open();

    return await GetUserAsync(userId, connection);
  }

  public async Task UpdateBalanceAsync(UpdateBalanceResponse response)
  {
    using var connection = _commandProvider.Open();
    using var transaction = connection.BeginTransaction();

    try
    {
      var user = await GetUserAsync(response.UserId, connection);

      if (user == null)
      {
        response.Status = HttpStatusCode.NotFound;
        response.Message = "User not found.";
        return;
      }

      if (user.Balance + response.UpdateAmount < 0)
      {
        response.Status = HttpStatusCode.BadRequest;
        response.Message = "User balance cannot be less than zero.";
        return;
      }

      const string updateUserSql = @"
        UPDATE tb_Users 
        SET Balance = Balance + @UpdateAmount 
        WHERE UserId = @UserId 
          AND Balance + @UpdateAmount >= 0;
        
        SELECT Balance 
        FROM tb_Users 
        WHERE UserId = @UserId;";
      var balance = await connection.ExecuteScalarAsync<decimal?>(updateUserSql, response);

      response.Balance = balance.Value;

      const string updateLogSql = "INSERT INTO tb_BalanceUpdateLog (UserId, UpdateAmount, Balance) VALUES (@UserId, @UpdateAmount, @Balance)";
      var rowsAdded = await connection.ExecuteAsync(updateLogSql, response);

      if (rowsAdded == 0)
      {
        response.Status = HttpStatusCode.InternalServerError;
        response.Message = "Unable to update Balance Log.";
        return;
      }

      transaction.Commit();
      response.Status = HttpStatusCode.OK;
      response.Message = "Balance updated successfully.";
    }
    catch (Exception ex)
    {
      response.Status = HttpStatusCode.InternalServerError;
      response.Message = "An error occurred while updating the balance.";
      transaction.Rollback();
    }
  }

  private static async Task<CasinoUser?> GetUserAsync(int userId, IUserConnection connection)
  {
    const string sql = "SELECT UserId, Username, Password, Email, HomePhoneNumber, WorkPhoneNumber, MobilePhoneNumber, Balance FROM tb_Users WHERE UserId = @UserId";
    var parameters = new { UserId = userId };

    var user = await connection.QuerySingleOrDefaultAsync<CasinoUser>(sql, parameters);

    return user;
  }
}
