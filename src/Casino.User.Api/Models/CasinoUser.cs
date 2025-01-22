namespace Casino.User.Api.Models
{
  public class CasinoUser
  {
    public int? UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? HomePhoneNumber { get; set; }

    public string? WorkPhoneNumber { get; set; }

    public string? MobilePhoneNumber { get; set; }

    public decimal? Balance { get; set; } = 0;
  }
}
