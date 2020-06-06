using System.ComponentModel.DataAnnotations;

namespace AuthServer.Contracts.V2.Requests
{
  public class UserRegistrationRequest
  {
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
  }
}