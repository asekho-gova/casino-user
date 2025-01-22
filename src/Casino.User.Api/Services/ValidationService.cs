using Casino.User.Api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Casino.User.Api.Services
{
  public class ValidationService : IValidationService
  {
    public string? ValidateRequest(CreateUserRequest request)
    {
      if (string.IsNullOrWhiteSpace(request.Username))
      {
        return "Username is required.";
      }

      if (!IsValidPassword(request.Password))
      {
        return "Password should be at least 8 characters long and contain at least one uppercase, one lowercase and one digit.";
      }

      if (string.IsNullOrWhiteSpace(request.Email))
      {
        return "Email is required.";
      }

      if (!IsValidEmail(request.Email))
      {
        return "Email is not in a valid format.";
      }

      if (IsAtLeastOnePhoneNumberSupplied(request))
      {
        return "At least one phone number is required.";
      }

      if (!IsValidPhoneNumber(request.HomePhoneNumber))
      {
        return "Home phone number is not in a valid format.";
      }

      if (!IsValidPhoneNumber(request.WorkPhoneNumber))
      {
        return "Work phone number is not in a valid format.";
      }

      if (!IsValidPhoneNumber(request.MobilePhoneNumber))
      {
        return "Mobile phone number is not in a valid format.";
      }

      return null;
    }

    private static bool IsAtLeastOnePhoneNumberSupplied(CreateUserRequest request)
    {
      return string.IsNullOrWhiteSpace(request.HomePhoneNumber) &&
             string.IsNullOrWhiteSpace(request.WorkPhoneNumber) &&
             string.IsNullOrWhiteSpace(request.MobilePhoneNumber);
    }

    private static bool IsValidEmail(string email)
    {
      return MailAddress.TryCreate(email, out _);
    }

    private static bool IsValidPassword(string? password)
    {
      if (string.IsNullOrWhiteSpace(password))
      {
        return false;
      }

      return MeetsLengthRequirements(password) && HasRequiredCharacters(password);
    }

    private static bool MeetsLengthRequirements(string password)
    {
      const int MIN_LENGTH = 8;
      const int MAX_LENGTH = 256;

      return password.Length >= MIN_LENGTH && password.Length <= MAX_LENGTH;
    }

    private static bool HasRequiredCharacters(string password)
    {
      var hasUpperCaseLetter = false;
      var hasLowerCaseLetter = false;
      var hasDecimalDigit = false;

      foreach (var c in password)
      {
        if (char.IsUpper(c))
        {
          hasUpperCaseLetter = true;
        }
        else if (char.IsLower(c))
        {
          hasLowerCaseLetter = true;
        }
        else if (char.IsDigit(c))
        {
          hasDecimalDigit = true;
        }

        if (hasUpperCaseLetter && hasLowerCaseLetter && hasDecimalDigit)
        {
          return true;
        }
      }

      return false;
    }

    private static bool IsValidPhoneNumber(string? phoneNumber)
    {
      if (string.IsNullOrWhiteSpace(phoneNumber))
      {
        return true;
      }

      var phoneRegex = new Regex(@"^\+?[0-9a]\d{6,14}$", RegexOptions.None, TimeSpan.FromMilliseconds(100));
      return phoneRegex.IsMatch(phoneNumber);
    }

    public bool ValidateUserId(int userId)
    {
      return userId >= 0;
    }

    public string? ValidateUpdateBalanceAmount(decimal amount)
    {
      if (amount == 0)
      {
        return "Cannot update the balance amount with zero.";
      }

      if (decimal.Round(amount, 3) != amount)
      {
        return "Cannot update the balance amount with fractional cents.";
      }

      return null;
    }

    public static void HashPassword(CreateUserRequest request)
    {
      var salt = RandomNumberGenerator.GetBytes(128 / 8);

      var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
          password: request.Password!,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: 100000,
          numBytesRequested: 256 / 8));

      request.HashedPassword = hashed;
    }
  }
}
