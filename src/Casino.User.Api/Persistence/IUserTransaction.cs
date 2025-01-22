namespace Casino.User.Api.Persistence
{
  public interface IUserTransaction : IDisposable
  {
    void Commit();
    void Rollback();
  }
}
