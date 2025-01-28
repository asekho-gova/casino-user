using Casino.User.Api.Models;

namespace Casino.User.Api.Services
{
  public interface IUserService
  {
    Task<CasinoUser?> GetUserAsync(int userId);
    Task<CasinoUser?> CreateUserAsync(CreateUserRequest request);
    Task<UpdateBalanceResponse> UpdateBalanceAsync(int userId, decimal amount);
    Task<bool> DeleteUserAsync(int userId);
  }
}
