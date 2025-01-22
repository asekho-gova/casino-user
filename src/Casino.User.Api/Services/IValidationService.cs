using Casino.User.Api.Models;

namespace Casino.User.Api.Services
{
  public interface IValidationService
  {
    string? ValidateRequest(CreateUserRequest request);
    string? ValidateUpdateBalanceAmount(decimal amount);
    bool ValidateUserId(int userId);
  }
}
