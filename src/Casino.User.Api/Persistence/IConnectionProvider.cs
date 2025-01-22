namespace Casino.User.Api.Persistence
{
  public interface IConnectionProvider
  {
    IUserConnection Open();
  }
}
