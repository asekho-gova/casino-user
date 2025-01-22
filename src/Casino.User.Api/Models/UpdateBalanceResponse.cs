using System.Net;

namespace Casino.User.Api.Models
{
  public class UpdateBalanceResponse
  {
    public int UserId { get; set; }

    public HttpStatusCode Status { get; set; }

    public string? Message { get; set; }

    public decimal UpdateAmount { get; set; }

    public decimal Balance { get; set; }
  }
}
