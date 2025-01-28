using Casino.User.Api.Models;

namespace Casino.User.Api.Persistence
{
    public interface IDatabaseService
    {
        Task<CasinoUser?> GetUserAsync(int userId);
        Task<int> CreateUserAsync(CreateUserRequest request);
        Task UpdateBalanceAsync(UpdateBalanceResponse response);
        Task<IUserTransaction> BeginTransactionAsync();
    }
}
