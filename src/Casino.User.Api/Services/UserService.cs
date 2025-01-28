using Casino.User.Api.Models;
using Casino.User.Api.Persistence;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Casino.User.Api.Services
{
    public class UserService : IUserService
  {
    private readonly IDatabaseService _databaseContext;
    private readonly IValidationService _validationService;

    public UserService(IValidationService validationService,
                           IDatabaseService databaseContext)
    {
      _validationService = validationService;
      _databaseContext = databaseContext;
    }

    public async Task<CasinoUser?> CreateUserAsync(CreateUserRequest request)
    {
      var validation = _validationService.ValidateRequest(request);
      if (!string.IsNullOrEmpty(validation))
      {
        throw new ValidationException(validation);
      }

      ValidationService.HashPassword(request);

      var userId = await _databaseContext.CreateUserAsync(request);

      return await _databaseContext.GetUserAsync(userId);
    }

    public async Task<CasinoUser?> GetUserAsync(int userId)
    {
      var validUser = _validationService.ValidateUserId(userId);
      if (!validUser)
      {
        return null;
      }

      return await _databaseContext.GetUserAsync(userId);
    }

    public async Task<UpdateBalanceResponse> UpdateBalanceAsync(int userId, decimal amount)
    {
      var response = new UpdateBalanceResponse()
      {
        UserId = userId,
        UpdateAmount = amount
      };

      try
      {
        var validUser = _validationService.ValidateUserId(userId);
        if (!validUser)
        {
          response.Status = HttpStatusCode.BadRequest;
          response.Message = "Invalid user id.";
          return response;
        }

        var validation = _validationService.ValidateUpdateBalanceAmount(amount);
        if (!string.IsNullOrEmpty(validation))
        {
          response.Status = HttpStatusCode.BadRequest;
          response.Message = validation;
          return response;
        }

        using (var transaction = await _databaseContext.BeginTransactionAsync())
        {
          await _databaseContext.UpdateBalanceAsync(response);
          if (response.Status == HttpStatusCode.OK)
          {
            transaction.Commit();
          }
          else
          {
            transaction.Rollback();
          }
        }
      }
      catch (Exception ex)
      {
        response.Status = HttpStatusCode.InternalServerError;
        response.Message = ex.Message;
      }

      return response;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
      var validUser = _validationService.ValidateUserId(userId);
      if (!validUser)
      {
        return false;
      }

      try
      {
        using (var transaction = await _databaseContext.BeginTransactionAsync())
        {
          var result = await _databaseContext.DeleteUserAsync(userId);
          if (result)
          {
            transaction.Commit();
            return true;
          }
          transaction.Rollback();
          return false;
        }
      }
      catch
      {
        return false;
      }
    }
  }
}
