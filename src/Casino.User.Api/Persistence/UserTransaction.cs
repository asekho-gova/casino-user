using System.Data;

namespace Casino.User.Api.Persistence
{
  public class UserTransaction : IUserTransaction
  {
    private readonly IDbTransaction _transaction;

    public UserTransaction(IDbTransaction transaction)
    {
      _transaction = transaction;
    }

    public void Commit()
    {
      _transaction.Commit();
    }

    public void Rollback()
    {
      _transaction.Rollback();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      _transaction.Dispose();
    }
  }
}
