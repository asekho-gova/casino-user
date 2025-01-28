using System.Text.Json.Serialization;

namespace Casino.User.Api.Models
{
  public class CreateUserRequest
  {
    public string? Username { get; set; }

    public string? Password { get; set; }

    [JsonIgnore]
    public string? HashedPassword { get; set; }
    public string? Salt { get; set; }

    public string? Email { get; set; }

    public string? HomePhoneNumber { get; set; }

    public string? WorkPhoneNumber { get; set; }

    public string? MobilePhoneNumber { get; set; }
  }
}
